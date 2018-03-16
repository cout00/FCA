using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Export;

namespace Core.FCA
{
    public class FCAAlgoritm
    {
        private readonly IExport _export;
        private readonly byte[,] Context;
        Dictionary<List<int>, List<int>> FormalContext = new Dictionary<List<int>, List<int>>();

        public FCAAlgoritm(IExport export)
        {
            _export = export;
            Context = export.ToMatrix();
        }


        int[] GetExtentByIntent(int intentIndex)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < Context.GetLength(0); i++)
            {
                if (Context[i, intentIndex] == 1)
                {
                    res.Add(i);
                }
            }
            return res.ToArray();
        }


        IEnumerable<int[]> NextExtent(int[] currentExtent)
        {
            var elementCount = (long)Math.Pow(2, currentExtent.Length);
            for (long i = elementCount - 1; i >= 0; i--)
            {
                List<int> tempRes = new List<int>();
                for (int j = 0; j < currentExtent.Length; j++)
                {
                    if (((i >> j) & 1) == 1)
                    {
                        tempRes.Add(currentExtent[j]);
                    }
                }
                if (tempRes.Count > 1)
                {
                    yield return tempRes.ToArray();
                }
            }
        }

        void FindRecurse(int[] nextExtent, int intentNumber)
        {
            List<int> tempContext = new List<int>(); ;
            foreach (var intent in NextExtent(nextExtent))
            {
                tempContext = new List<int>();
                for (int i = 0; i < Context.GetLength(1); i++)
                {
                    var theyAllTrue = true;
                    for (int k = 0; k < intent.Length; k++)
                    {
                        theyAllTrue &= Context[intent[k], i] == 1;
                        if (!theyAllTrue)
                        {
                            break;
                        }
                    }
                    if (theyAllTrue)
                    {
                        tempContext.Add(i);
                    }
                }
                if (tempContext.Count > 1)
                {
                    if (FormalContext.Where(pair => pair.Value.Except(tempContext).Count() == 0 && tempContext.Except(pair.Value).Count() == 0 && pair.Key.Count >= intent.Length).Count() == 0)
                    {
                        FormalContext.Add(intent.ToList(), tempContext);
                    }

                }
            }
            if (intentNumber < Context.GetLength(1) - 1)
            {
                FindRecurse(GetExtentByIntent(intentNumber + 1), intentNumber + 1);
            }
        }




        RelatedConcepts Process()
        {
            for (int i = 0; i < Context.GetLength(1); i++)
            {
                var res = GetExtentByIntent(i);
                if (res.Length > 1)
                {
                    FindRecurse(res, i);
                    break;
                }
            }
            RelatedConcepts concepts=new RelatedConcepts();
            foreach (var formalContext in FormalContext)
            {
                List<InternalObject> objects = formalContext.Key.Select(value =>
                {
                    return new InternalObject() {Index = value, Name = _export.GetObjects()[value]};
                }).ToList();
                List<InternalObject> attributes = formalContext.Value.Select(value => {
                    return new InternalObject() { Index = value, Name = _export.GetAttributes()[value] };
                }).ToList();
                concepts.Add(objects,attributes);
            }
            return concepts;
        }


        public async Task<RelatedConcepts> RunFCAAsync()
        {
            TaskFactory<RelatedConcepts> factory=new TaskFactory<RelatedConcepts>();
            return await factory.StartNew(Process);             
        }




    }
}
