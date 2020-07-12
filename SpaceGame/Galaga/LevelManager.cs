using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaga
{
    class LevelManager
    {
        private uint enemyNumber;
        private uint level;

        public LevelManager()
        {
            level = 0;
        }

        /*
         * Reset to Level One
         */
        public void Reset()
        {
            level = 0;
        }

        public void BeatLevel()  { level++; }

        public uint GetEnemyNumber 
        { 
            get { return level; } 
        }

        public uint GetLevel
        {
            get { return level; }
        }

        public uint GetAttackers
        {
            get { return (uint) Math.Ceiling((float)level / 2.0); }
        }
    }
}
