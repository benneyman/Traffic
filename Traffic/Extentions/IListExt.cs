using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Interface;

namespace Traffic.Extentions
{
    public static class IListExt
    {
        public static void NextPermutation(this List<ICity> cities)
        {
            //Find the first instance of where a[i] < a[i + 1]
            if (cities.Count < 2)
                return;
            int lastIndex = cities.Count - 1, currentIndex = lastIndex - 1;
            while (lastIndex > 0)
            {
                currentIndex = lastIndex - 1;
                if(cities[currentIndex].CityId < cities[lastIndex].CityId)
                {
                    break;
                }
                --lastIndex;
            }
            if (lastIndex == 0)
            {
                cities.Reverse();
                return;
            }
            //Find index k where arr[k] > cuurentIndex 
            int k = cities.Count - 1;
            while(cities[k].CityId < cities[currentIndex].CityId)
            {
                --k;
            }
            cities.Swap(currentIndex, k);
            cities.Reverse(currentIndex + 1, cities.Count - (currentIndex + 1));
        }

        public static void Swap<T>(this List<T> input, int i, int j)
        {
            if (i >= input.Count || j >= input.Count)
                throw new IndexOutOfRangeException();
            T temp = input[i];
            input[i] = input[j];
            input[j] = temp;
        }
    }
}
