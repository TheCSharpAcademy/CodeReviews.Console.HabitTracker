using System;
using System.ComponentModel.DataAnnotations;

namespace FlashCards
{
    enum Ports
    {
        SSH = 22,
        DNS = 53,
        SFTP = 22,
        FTP1 = 20,
        FTP2 = 21,
        TFTP = 69,
        TELNET = 23,
        DHCP1 = 67,
        DHCP2 = 68,
        HTTP = 80,
        HTTPS = 443,
        SNMP1 = 161, 
        SNMP2 = 162,
        RDP = 3389,
        NTP = 123,
        SIP1 = 5060, 
        SIP2 = 5061,
        SMB = 445,
        SMTP = 25,
        SecureSMTP = 587,
        POP3 = 110,
        SecurePOP3 = 995,
        IMAP = 143,
        SecureIMAP = 993,
        LDAP = 389,
        LDAPS = 636,
        Syslog = 514,
        SQLServer = 1433,
        Oracle = 1521,
        MySQL = 3306
    }

    class Program
    {
        static void Main()
        {
            Random random = new Random();
            int max = Enum.GetNames(typeof(Ports)).Length;
            int n;
            string? answer;
            Array values = Enum.GetValues(typeof(Ports));

            do
            {
                n = random.Next(values.Length);
                Ports randomPort = (Ports)values.GetValue(n);
                Ports p = randomPort;
                int port = (int)p;
                string r = port.ToString();

                Console.WriteLine($"What is port number of {randomPort}?");             
                
                answer = Console.ReadLine();

                try
                {
                    check(answer!, r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (answer != null);

            static void check(string input, string ans)
            {
                if (input == ans)
                {
                    Console.WriteLine("Your answer was correct! Type any key for the next question");
                }
                else
                    Console.WriteLine($"Incorrect. Correct answer is {ans}. Type any key for next question");
                Console.ReadLine();
            }
        }
    }
}
