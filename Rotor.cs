using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Rotor
    {
        private char name;
        private string permutation;
        private int RotorLength;
        private char[] rotorCharSet;
        private char[] rotorCharSetReverse;
        private int offset;
        private char rotorSetting;
        private int turnoverNotch;
        private bool isShift;

        /*---------------------Rotor Constructors--------------------*/
        public Rotor()
        {
            this.name = '1';
            this.permutation = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.RotorLength = this.permutation.Length;
            this.rotorCharSet = permutation.ToCharArray();
            this.rotorCharSetReverse = permutation.ToCharArray();
            Array.Reverse(rotorCharSetReverse);
            this.offset = 0;
            this.rotorSetting = 'A';
            this.turnoverNotch = 25;
            this.isShift = false;
        }
        public Rotor(char _name, string _permutation, int _offset, int _turnoverNotch, char _rotorSetting)
        {
            this.name = _name;
            this.permutation = _permutation;
            this.RotorLength = this.permutation.Length;
            this.rotorCharSet = permutation.ToCharArray();
            this.rotorCharSetReverse = permutation.ToCharArray();
            Array.Reverse(rotorCharSetReverse);
            this.offset = _offset;
            this.rotorSetting = _rotorSetting;
            this.turnoverNotch = _turnoverNotch;
            this.isShift = false;
        }
        /*------------------End of Rotor Constructors----------------*/

        /*---------------------Setters & Getters---------------------*/
        /*Set & Get Rotor Name*/
        public void setRotorName(char _name)
        {
            this.name = _name;
        }
        public char getRotorName()
        {
            return this.name;
        }
        /*End of Set & Get Rotor Name*/

        /*Set & Get Rotor Permutation*/
        public void setRotorPermutation(string _permutation)
        {
            this.permutation = _permutation;
        }
        public string getRotorPermutation()
        {
            return this.permutation;
        }
        /*End of Set & Get Rotor Permutation*/

        /*Set & Get Rotor Permutation*/
        public void setRotorArrays()
        {
            this.RotorLength = this.permutation.Length;
            this.rotorCharSet = permutation.ToCharArray();
            //this.rotorCharSetReverse = ("UWYGADFPVZBECKMTHXSLRINQOJ").ToCharArray();
            reverseRotorArray();
        }
        /*End of Set & Get Rotor Permutation*/

        /*Set & Get Rotor Offset*/
        public void setRotorOffset(int _offset)
        {
            this.offset = _offset;
        }
        public int getRotorOffset()
        {
            return this.offset;
        }
        /*End of Set & Get Rotor Offset*/

        /*Set & Get Rotor Setting*/
        public void setRotorSetting(char _rotorSetting)
        {
            this.rotorSetting = _rotorSetting;
        }
        public char getRotorSetting()
        {
            return this.rotorSetting;
        }
        /*End of Set & Get Rotor Setting*/

        /*Set & Get Rotor Turnover Notch*/
        public void setRotorTurnoverNotch(int _turnoverNotch)
        {
            this.turnoverNotch = _turnoverNotch;
        }
        public int getRotorTurnoverNotch()
        {
            return this.turnoverNotch;
        }
        /*End of Set & Get Rotor Turnover Notch*/

        /*Set & Get isShift*/
        public void setIsShift(bool _isShift)
        {
            this.isShift = _isShift;
        }
        public bool getIsShift()
        {
            return this.isShift;
        }
        /*End of Set & Get Rotor Turnover Notch*/
        /*------------------End of Setters & Getters------------------*/

        /*---------------------Utilities Functions--------------------*/
        public char encryptChar(char charToEncrypt, bool needToShift)
        {
            this.isShift = false;
            if (getRotorOffset() == getRotorTurnoverNotch())
            {
                this.isShift = true;
            }
            if (needToShift || this.isShift==true)
            {
                shiftOffset();
            }


            charToEncrypt = permutation[changeIfNegetive((charToInt(charToEncrypt) +
                                        this.offset -
                                        charToInt(this.rotorSetting))%26)];

            return intToChar(changeIfNegetive((charToInt(charToEncrypt)-
                             this.offset+
                             charToInt(this.rotorSetting))%26));
        }

        public char encryptCharRevers(char charToEncrypt)
        {

            charToEncrypt = rotorCharSetReverse[changeIfNegetive((charToInt(charToEncrypt) +
                                        this.offset -
                                        charToInt(this.rotorSetting)) % 26)];

            return intToChar(changeIfNegetive((charToInt(charToEncrypt) -
                             this.offset +
                             charToInt(this.rotorSetting)) % 26));
        }

        private void shiftOffset()
        {
            this.offset=(this.offset+1) %26;
        }
        private void rotorCharSetUpdate(string text)
        {
            this.rotorCharSet = text.ToCharArray();
        }
        private char getOffsetChar()
        {
            return this.rotorCharSet[this.offset];
        }
        public bool isNotch(char charToEncrypt)
        {
            if(charToInt(charToEncrypt) == this.turnoverNotch)
            {
                return true;
            }
            return false;
        }
        private int charToInt(char charToConvert)
        {
            return (int)(charToConvert) - 65;
        }
        private char intToChar(int intToConvert)
        {
            return (char)(intToConvert+65);
        }
        private int changeIfNegetive(int _num)
        {
            if (_num < 0)
                return _num + 26;
            return _num;
        }
        private void reverseRotorArray()
        {
            for(int i=0;i< RotorLength;i++)
            {
                rotorCharSetReverse[i] = reverseRotorChar(rotorCharSet[i]);
            }
        }
        private char reverseRotorChar(char charToConvert)
        {
            int newIndex=0;
            for (int i = 0; i < RotorLength; i++)
            {
                if (rotorCharSet[i] == charToConvert)
                {
                    newIndex = i ;
                    break;
                }
            }

            char reverseChar = intToChar(newIndex);

            for (int i = 0; i < RotorLength; i++)
            {
                if (rotorCharSet[i] == reverseChar)
                {
                    newIndex = i ;
                    break;
                }
            }
            return intToChar(newIndex);
        }
        /*-----------------End of Utilities Functions------------------*/

        //private void shiftRotorEncrypt()
        //{
        //    this.permutation = this.permutation.Substring(this.permutation.Length - 1, 1)
        //        + this.permutation.Substring(0, this.permutation.Length - 1);
        //    rotorCharSetUpdate(this.permutation);
        //}

        //private void shiftRotorDecrypt()
        //{
        //    this.permutation = this.permutation.Substring(1, this.permutation.Length - 1)
        //        + this.permutation.Substring(0, 1);
        //    rotorCharSetUpdate(this.permutation);
        //}
    }
}
