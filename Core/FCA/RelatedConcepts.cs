using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FCA
{
    public class RelatedConcepts :Dictionary<List<InternalObject>, List<InternalObject>>
    {
        public TreeNode<NodeData> GetTree()
        {
            NodeData data=new NodeData();
            TreeNode<NodeData> rootNode =new TreeNode<NodeData>(data);
            string text = "Concept number ";
            var i = 1;
            foreach (var relatedConcept in this)
            {
                Dictionary<int, List<int>> topLevelData=new Dictionary<int, List<int>>();
                NodeData rootData=new NodeData();
                rootData.IsTopLevel = true;
                rootData.Data = topLevelData;
                rootData.Text = text + ++i;
                var childChildNode = rootNode.AddChild(rootData);
                foreach (var internalObject in relatedConcept.Key)
                {
                    topLevelData.Add(internalObject.Index, relatedConcept.Value.Select(o => o.Index).ToList());
                    NodeData childData=new NodeData();
                    childData.Text = internalObject.Name;
                    var childTemp = new Dictionary<int, List<int>>();
                    childTemp.Add(internalObject.Index, relatedConcept.Value.Select(o => o.Index).ToList());
                    childData.Data = childTemp;
                    var attNode= childChildNode.AddChild(childData);
                    foreach (var atts in relatedConcept.Value)
                    {
                        NodeData attNodeData=new NodeData();
                        attNodeData.Text = atts.Name;
                        var attemp = new Dictionary<int, List<int>>();
                        attNodeData.Data = attemp;
                        attemp.Add(internalObject.Index, relatedConcept.Value.Select(o => o.Index).ToList());
                        attNode.AddChild(attNodeData);
                    }
                }
            }
            return rootNode;

        }
    }
}
