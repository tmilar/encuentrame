using System.Collections.Generic;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Newtonsoft.Json;
using Encuentrame.Model.Accounts.Permissions;

namespace Encuentrame.Web.Helpers
{
    public static class ListTreeItemsHelper
    {
        [Inject]
        public static IBag<Pass> Passes { get; set; }

        

        public static IList<Node> GetPasses(IEnumerable<int> ids )
        {
            var passes = Passes.Where(x=>true);
            if (ids.Any())
            {
                passes = passes.Where(x => ids.Contains(x.Id));     
            }
           

            var list = new List<Node>();

            Node groupNode = null;
            Node moduleNode = null;

            GroupsOfModulesEnum lastGroup = (GroupsOfModulesEnum)0;
            ModulesEnum lastModule = (ModulesEnum)0;

            foreach (var item in passes.OrderBy(x => x.Group).ThenBy(x => x.Module).ThenBy(x => x.Action))
            {

                if (groupNode == null || lastGroup != item.Group)
                {
                    lastGroup = item.Group;
                    groupNode = new Node { Text = TranslationsHelper.Get(item.Group) };

                    list.Add(groupNode);
                }

                if (moduleNode == null || lastModule != item.Module)
                {
                    lastModule = item.Module;
                    moduleNode = new Node { Text = TranslationsHelper.Get(item.Module) };
                    groupNode.Nodes.Add(moduleNode);
                }

                var actionNode = new Node { Text = TranslationsHelper.Get(item.Action), Value = item.Id, Checked = false };
                moduleNode.Nodes.Add(actionNode);
            }

            return list;
        }

        public class Node
        {
            public Node()
            {
                this.Nodes = new List<Node>();
            }
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("value")]
            public int Value { get; set; }

            [JsonProperty("checked")]
            public bool Checked { get; set; }

            [JsonProperty("nodes")]
            public IList<Node> Nodes { get; set; }
        }

    }
}