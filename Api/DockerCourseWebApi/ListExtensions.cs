using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace DockerCourseWebApi
{

    public static class ListExtensions
    {
        public static List<T>? Randomize<T>(this List<T> list)
        {
            if (list == null || !list.Any())
            {
                return list;
            }

            var span = CollectionsMarshal.AsSpan(list);

            ///<summary>
            ////Fisher-Yates shuffle algorithm using RandomGenerator.GetInt32 for cryptographically secure random numbers
            //and using tuple deconstruction to swap elements in the span
            //and spans to avoid unnecessary allocations and improve performance
            /// </summary>
            static void Shuffle(Span<T> span)
            {
                
                for (int i = span.Length - 1; i > 0; i--)
                {
                    int j = RandomNumberGenerator.GetInt32(i + 1);
                    (span[i], span[j]) = (span[j], span[i]);
                }
            }

            Shuffle(span); 

            return list;
        }

    }    

}
