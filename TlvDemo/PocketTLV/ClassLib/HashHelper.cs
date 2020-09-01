using System;
using System.Collections.Generic;

namespace PocketTlv.ClassLib
{
    // Thanks StackOverflow!
    // http://stackoverflow.com/a/26493039/344638

    /// <summary>
    /// Provides helper methods to compute hash codes correctly when implementing GetHashCode.
    /// </summary>
    public static class HashHelper
    {
        private const int PrimeOne = unchecked((int)2166136261);
        private const int PrimeTwo = unchecked(16777619);

        public static int GetHashCode<T>( IList<T> list )
        {
            unchecked
            {
                int hash = PrimeOne;

                foreach( var element in list )
                {
                    hash = hash * PrimeTwo + element.GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>( T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();
                hash = hash * PrimeTwo + arg7.GetHashCode();
                hash = hash * PrimeTwo + arg8.GetHashCode();
                hash = hash * PrimeTwo + arg9.GetHashCode();
                hash = hash * PrimeTwo + arg10.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9>( T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();
                hash = hash * PrimeTwo + arg7.GetHashCode();
                hash = hash * PrimeTwo + arg8.GetHashCode();
                hash = hash * PrimeTwo + arg9.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8>( T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();
                hash = hash * PrimeTwo + arg7.GetHashCode();
                hash = hash * PrimeTwo + arg8.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>( T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();
                hash = hash * PrimeTwo + arg7.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2, T3, T4, T5, T6>( T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2, T3, T4, T5>( T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2, T3, T4>( T1 arg1, T2 arg2, T3 arg3, T4 arg4 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2, T3>( T1 arg1, T2 arg2, T3 arg3 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Computes the hashcode of the given arguments.
        /// </summary>
        public static int GetHashCode<T1, T2>( T1 arg1, T2 arg2 )
        {
            unchecked
            {
                int hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();

                return hash;
            }
        }
    }
}