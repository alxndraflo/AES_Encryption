using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Aes128
{
    class Aes128
    {
        //Data Members
        private readonly KeySchedule keySchedule = new KeySchedule();

        //Hard coded s-box from prof
	    private byte[] sBox =
	    {
		    0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
		    0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
		    0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
		    0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
		    0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
		    0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
		    0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
		    0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
		    0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
		    0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
		    0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
		    0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
		    0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
		    0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
		    0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
		    0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
	    };

        //Hard coded Key schedule from prof
        private byte[,] keys =
        {
            {0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c},
            {0xa0, 0xfa, 0xfe, 0x17, 0x88, 0x54, 0x2c, 0xb1, 0x23, 0xa3, 0x39, 0x39, 0x2a, 0x6c, 0x76, 0x05},
            {0xf2, 0xc2, 0x95, 0xf2, 0x7a, 0x96, 0xb9, 0x43, 0x59, 0x35, 0x80, 0x7a, 0x73, 0x59, 0xf6, 0x7f},
            {0x3d, 0x80, 0x47, 0x7d, 0x47, 0x16, 0xfe, 0x3e, 0x1e, 0x23, 0x7e, 0x44, 0x6d, 0x7a, 0x88, 0x3b},
            {0xef, 0x44, 0xa5, 0x41, 0xa8, 0x52, 0x5b, 0x7f, 0xb6, 0x71, 0x25, 0x3b, 0xdb, 0x0b, 0xad, 0x00},
            {0xd4, 0xd1, 0xc6, 0xf8, 0x7c, 0x83, 0x9d, 0x87, 0xca, 0xf2, 0xb8, 0xbc, 0x11, 0xf9, 0x15, 0xbc},
            {0x6d, 0x88, 0xa3, 0x7a, 0x11, 0x0b, 0x3e, 0xfd, 0xdb, 0xf9, 0x86, 0x41, 0xca, 0x00, 0x93, 0xfd},
            {0x4e, 0x54, 0xf7, 0x0e, 0x5f, 0x5f, 0xc9, 0xf3, 0x84, 0xa6, 0x4f, 0xb2, 0x4e, 0xa6, 0xdc, 0x4f},
            {0xea, 0xd2, 0x73, 0x21, 0xb5, 0x8d, 0xba, 0xd2, 0x31, 0x2b, 0xf5, 0x60, 0x7f, 0x8d, 0x29, 0x2f},
            {0xac, 0x77, 0x66, 0xf3, 0x19, 0xfa, 0xdc, 0x21, 0x28, 0xd1, 0x29, 0x41, 0x57, 0x5c, 0x00, 0x6e},
            {0xd0, 0x14, 0xf9, 0xa8, 0xc9, 0xee, 0x25, 0x89, 0xe1, 0x3f, 0x0c, 0xc8, 0xb6, 0x63, 0x0c, 0xa6}
        };

        //Hard coded plaintext provided by prof
        private byte[] plaintext = new byte[16];

        //2D byte array to store state at each step 
        private byte[,] state = new byte[4, 4];


        //Constructor
        public Aes128(byte[] plaintextIn)
        {
            this.plaintext = plaintextIn;
            this.state = ConvertToColMajorMatrix(plaintext);
        }


        //Public Methods

        //Public encrypt method - Drives encryption -- calls other private methods
        public void Encrypt()
        {
            //Initial plaintext
            Console.WriteLine("== Plaintext ==");
            string stateString = PrintState();
            Console.WriteLine(stateString);

            //Round 0: INITIAL Round
            byte[,] roundKey = ConvertToColMajorMatrix(keySchedule.GetKey(0));
            AddRoundKey(0, roundKey);
            //Console.WriteLine(PrintState());

            //Rounds 1-9
            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine("== Round {0} ==", i);
                Console.WriteLine("-- Start of Round --");
                Console.WriteLine(PrintState());

                //Do SubBytes
                SubBytes();

                //Print State
                Console.WriteLine("-- After SubBytes --");
                Console.WriteLine(PrintState());

                //Do ShiftRows
                ShiftRows();

                //Print State
                Console.WriteLine("-- After ShiftRows --");
                Console.WriteLine(PrintState());

                //Do MixColumns
                MixColumns();

                //Print State
                Console.WriteLine("-- After MixColumns --");
                Console.WriteLine(PrintState());

                //Do AddRoundKey
                roundKey = ConvertToColMajorMatrix(keySchedule.GetKey(i));
                AddRoundKey(i, roundKey);
            }

            //Round 10 - Final Round
            Console.WriteLine("== Round 10 ==");
            Console.WriteLine("-- Start of Round --");
            Console.WriteLine(PrintState());

            //Do SubBytes
            SubBytes();

            //Print State
            Console.WriteLine("-- After SubBytes --");
            Console.WriteLine(PrintState());

            //Do ShiftRows
            ShiftRows();

            //Print State
            Console.WriteLine("-- After ShiftRows --");
            Console.WriteLine(PrintState());

            //Do AddRoundKey
            roundKey = ConvertToColMajorMatrix(keySchedule.GetKey(10));
            AddRoundKey(10, roundKey);

            Console.WriteLine("== Ciphertext ==");
            Console.WriteLine(PrintState());
        }


        //Private Methods

        //Private helper method to convert 1d byte array to column major 2d byte array
        private byte[,] ConvertToColMajorMatrix(byte[] toConvert)
        {
            byte[,] tmpMatrix = new byte[4,4];
            int index = 0;

            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    tmpMatrix[row, col] = toConvert[index];
                    index++;
                }
            }

            return tmpMatrix;
        }

        //Method to create a string in 4x4 matrix table format for state
        private string PrintState()
        {
            StringBuilder stateString = new StringBuilder();

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    stateString.AppendFormat("{0:x2} ", this.state[r, c]);

                    if (c == 3)
                    {
                        stateString.AppendFormat("\n");
                    }
                }
            }

            stateString.AppendFormat("\n");

            return stateString.ToString();
        }

        //Method to perform byte substitutions
        private void SubBytes()
        {
            int row;
            int col;

            for (row = 0; row < 4; row++)
            {
                for (col = 0; col < 4; col++)
                {
                    state[row, col] = sBox[state[row, col]];
                }
            }
        }

        //Method to perform shifting of rows in state
        private void ShiftRows()
        {
            byte tmpPosition = state[1, 0];

            //Second Row
            state[1, 0] = state[1, 1];
            state[1, 1] = state[1, 2];
            state[1, 2] = state[1, 3];
            state[1, 3] = tmpPosition;

            //Third Row
            tmpPosition = state[2, 0];
            state[2, 0] = state[2, 2];
            state[2, 2] = tmpPosition;
            tmpPosition = state[2, 1];
            state[2, 1] = state[2, 3];
            state[2, 3] = tmpPosition;

            //Fourth Row
            tmpPosition = state[3, 0];
            state[3, 0] = state[3, 3];
            state[3, 3] = state[3, 2];
            state[3, 2] = state[3, 1];
            state[3, 1] = tmpPosition;
        }

        //Method to XOR the state and round key (subkey for given round)
        private void AddRoundKey(int round, byte[,] key)
        {
            int row;
            int col;

            for (row = 0; row < 4; row++)
            {
                for (col = 0; col < 4; col++)
                {
                    state[col,row] ^= key[((round * 4) % 4) + col, row];
                }
            }
        }

        //Method to mix columns in each round -- performs galois field multiplications on the state
        private void MixColumns()
        {
            byte[,] tmpState = new byte[4, 4];

            //First column
            tmpState[0, 0] = (byte)(MultiplyBy2(state[0,0]) ^ MultiplyBy3(state[1, 0]) ^ state[2, 0] ^ state[3, 0]);
            tmpState[1, 0] = (byte)(state[0, 0] ^ MultiplyBy2(state[1, 0]) ^ MultiplyBy3(state[2, 0]) ^ state[3, 0]);
            tmpState[2, 0] = (byte)(state[0, 0] ^ state[1, 0] ^ MultiplyBy2(state[2, 0]) ^ MultiplyBy3(state[3, 0]));
            tmpState[3, 0] = (byte)(MultiplyBy3(state[0, 0]) ^ state[1, 0] ^ state[2, 0] ^ MultiplyBy2(state[3, 0]));

            //Second column
            tmpState[0, 1] = (byte)(MultiplyBy2(state[0, 1]) ^ MultiplyBy3(state[1, 1]) ^ state[2, 1] ^ state[3, 1]);
            tmpState[1, 1] = (byte)(state[0, 1] ^ MultiplyBy2(state[1, 1]) ^ MultiplyBy3(state[2, 1]) ^ state[3, 1]);
            tmpState[2, 1] = (byte)(state[0, 1] ^ state[1, 1] ^ MultiplyBy2(state[2, 1]) ^ MultiplyBy3(state[3, 1]));
            tmpState[3, 1] = (byte)(MultiplyBy3(state[0, 1]) ^ state[1, 1] ^ state[2, 1] ^ MultiplyBy2(state[3, 1]));

            //Third column
            tmpState[0, 2] = (byte)(MultiplyBy2(state[0, 2]) ^ MultiplyBy3(state[1, 2]) ^ state[2, 2] ^ state[3, 2]);
            tmpState[1, 2] = (byte)(state[0, 2] ^ MultiplyBy2(state[1, 2]) ^ MultiplyBy3(state[2, 2]) ^ state[3, 2]);
            tmpState[2, 2] = (byte)(state[0, 2] ^ state[1, 2] ^ MultiplyBy2(state[2, 2]) ^ MultiplyBy3(state[3, 2]));
            tmpState[3, 2] = (byte)(MultiplyBy3(state[0, 2]) ^ state[1, 2] ^ state[2, 2] ^ MultiplyBy2(state[3, 2]));

            //Fourth column
            tmpState[0, 3] = (byte)(MultiplyBy2(state[0, 3]) ^ MultiplyBy3(state[1, 3]) ^ state[2, 3] ^ state[3, 3]);
            tmpState[1, 3] = (byte)(state[0, 3] ^ MultiplyBy2(state[1, 3]) ^ MultiplyBy3(state[2, 3]) ^ state[3, 3]);
            tmpState[2, 3] = (byte)(state[0, 3] ^ state[1, 3] ^ MultiplyBy2(state[2, 3]) ^ MultiplyBy3(state[3, 3]));
            tmpState[3, 3] = (byte)(MultiplyBy3(state[0, 3]) ^ state[1, 3] ^ state[2, 3] ^ MultiplyBy2(state[3, 3]));

            state = tmpState;
        }

        //Galois field matrix to use in multiplications - from FIPS doc
        private int[,] gMat =
        {
            {2, 3, 1, 1},
            {1, 2, 3, 1},
            {1, 1, 2, 3},
            {3, 1, 1, 2}
        };

        //Helper function from prof for galois field multiplications
        private int MultiplyBy2(int a)
        {
            int res = a << 1;

            if ((res & 0x100) != 0)
            {
                res ^= 0x11b;
            }

            return res;
        }

        //Helper function from prof for galois field multiplications
        private int MultiplyBy3(int a)
        {
            return MultiplyBy2(a) ^ a;
        }
    }
}
