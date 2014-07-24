using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class SessionInfo
    {
        public string SessionGuid { get; set; }
        public int ChunkCount { get; set; }
        public int EpochCount { get; set; }
        public int SkipSession { get; set; }
        public int SkipChunk { get; set; }
        public int SkipEpoch { get; set; }
        public List<int> SkipEpochList { get; set; }
      
        
        public void RandomizeEpoch()
        {
            this.SkipEpochList = new List<int>();
            for (int i = 0; i < ChunkCount; i++)
            {
                int value=Randomize(this.EpochCount, SkipEpoch);
                this.SkipEpochList.Add(value!=0? (value+(i*EpochCount)):0);            
            }         
        }

        public void RandomizeChunk()
        {
            this.SkipChunk = Randomize(this.ChunkCount,SkipChunk);
        }

        private int Randomize(int value,int skipNum)
        {
            if (skipNum == 0 || skipNum > value)
            {
                return 0;
            }
            else
            {
                Random random = new Random();
                int randomNo = random.Next(1, skipNum);
                return randomNo;
            }
        }

    }
}
