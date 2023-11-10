using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTproject
{
    internal class Mosters
    {
        public class Moster : ICharacter
        {
            public int Level { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public string Name => throw new NotImplementedException();

            public int Attack => throw new NotImplementedException();

            public int Defense { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int Health { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int Gold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public bool IsDead => throw new NotImplementedException();

            public void BeDamaged(int damage)
            {
                throw new NotImplementedException();
            }
        }
    }
}
