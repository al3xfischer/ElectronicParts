using ElectronicParts.Models;
using ElectronicParts.Services.Interfaces;
using Shared;
using System;
using System.Collections.Generic;

namespace ElectronicParts.Services.Implementations
{
    public class PinCreatorService : IPinCreatorService
    {
        public IPin CreatePin(Type type)
        {
            if (type == typeof(string))
            {
                return new Pin<string>();
            }
            else if (type == typeof(int))
            {
                return new Pin<int>();
            }
            else if (type == typeof(bool))
            {
                return new Pin<bool>();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IPin> CreatePins(Type type, int amout)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if(amout <= 0)
            {
                throw new ArgumentException("The amout must not be less than or equal to zero.");
            }

            for (int i = 0; i < amout; i++)
            {
                yield return this.CreatePin(type);
            }
        }
    }
}
