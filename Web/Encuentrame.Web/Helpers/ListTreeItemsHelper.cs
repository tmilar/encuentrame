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