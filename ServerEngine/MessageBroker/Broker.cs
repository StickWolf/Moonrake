using Amqp.Listener;
using System;
using System.Security.Cryptography.X509Certificates;

namespace ServerEngine.MessageBroker
{
    internal static class Broker
    {
        private static string address = "amqps://127.0.0.1:5671";
        private static ContainerHost Host { get; set; }

        public static void StartHost()
        {
            if (Host != null)
            {
                throw new InvalidOperationException("Host is already running.");
            }

            // Create listener and start it.
            Uri addressUri = new Uri(address);
            Host = new ContainerHost(addressUri);
            Host.Listeners[0].SSL.Certificate = GetSslCertificate();
            Host.Listeners[0].SASL.EnableAnonymousMechanism = true;
            // These next 2 lines are for test servers only!
            //Host.Listeners[0].SSL.ClientCertificateRequired = true;
            //Host.Listeners[0].SSL.RemoteCertificateValidationCallback = (a, b, c, d) => true;

            // TODO: implement a max concurrent connections
            Host.Open();
            System.Console.WriteLine($"MessageBroker Host is listenening on {addressUri.Host}:{addressUri.Port}");

            // Attach custom logic when links attempt to attach to the host
            Host.RegisterLinkProcessor(new LinkProcessor());
            System.Console.WriteLine($"MessageBroker link processor is now registered.");
        }

        private static X509Certificate2 GetSslCertificate()
        {
            var sslCertSubject = "CN=MoonrakeServer";
            // SSL self signed certificate creation notes.
            // 1. Start an admin powershell
            // 2. New-SelfSignedCertificate -DnsName "MoonrakeServer" -CertStoreLocation "cert:\LocalMachine\My" -KeySpec KeyExchange
            //    KeySpec is important, if missing the code won't be able to read the private key properly and you'll see errors like this in the system event log
            //    A fatal error occurred when attempting to access the TLS server credential private key. 
            //    The error code returned from the cryptographic module is 0x8009030D. The internal error state is 10001.
            //    Thanks to this SO for this hint: https://stackoverflow.com/questions/22581811/invalid-provider-type-specified-cryptographicexception-when-trying-to-load-pri
            // 3. Open MMC/Manage computer certs, locate the cert in personal and give the cert a friendly name if desired
            // 4. Right click on the certificate in LocalMachine\Personal and pick All Tasks->Manage Private Keys
            //    Add your current login explicitly here with full control.
            // 5. Copy the cert from Personal to Trusted Root Certification Authorities (Also in localmachine)

            // Get the ssl cert from the local cert store.
            X509Store store = new X509Store(StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            var foundCerts = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, sslCertSubject, false);
            if (foundCerts.Count == 0)
            {
                throw new Exception("Unable to find SSL certificate in the cert store.");
            }
            var sslCert = foundCerts[0];
            store.Close();
            return sslCert;
        }

        public static void StopHost()
        {
            Host?.Close();
        }
    }
}
