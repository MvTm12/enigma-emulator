using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Reflector
    {
        private char name;
        private string permutation;

        /*------Reflector Constructors------*/
        public Reflector()
        {
            this.name = 'B';
            this.permutation = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
        }
        public Reflector(char _name, string _permutation)
        {
            this.name = _name;
            this.permutation = _permutation;
        }
        /*------End of Reflector Constructors------*/

        /*Set & Get Reflector Name*/
        public char getName()
        {
            return this.name;
        }
        public void setName(char _name)
        {
            this.name = _name;
        }
        /*Set & Get Reflector Name*/

        /*Set & Get Reflector Permutation*/
        public string getReflectorPermutation()
        {
            return this.permutation;
        }
        public void setPermutation(string _permutation)
        {
            this.permutation = _permutation;
        }
        /*Set & Get Reflector Permutation*/


    }
}
