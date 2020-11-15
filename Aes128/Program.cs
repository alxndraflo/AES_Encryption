using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aes128
{
    class Program
    {
        static void Main(string[] args)
        {
            //Hard coded plaintext from FIPS197 document
	        byte[] plaintext =
				{0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34};

            //Create new aes object to encrypt plaintext
	        Aes128 aes = new Aes128(plaintext);

            //Do encryption
            aes.Encrypt();
        }
    }
}
