using NJsonSchema.CodeGeneration;
using SigSpec.Core;
using System.Collections.Generic;
using System.Linq;

namespace SigSpec.CodeGeneration.Models
{
    public class HubModel
    {
        private readonly string _path;
        private readonly SigSpecHub _hub;
        private readonly TypeResolverBase _resolver;

        public HubModel(string path, SigSpecHub hub, TypeResolverBase resolver)
        {
            this._path = path;
            this._hub = hub;
            this._resolver = resolver;
        }

        public string Name => this._hub.Name;

        public IEnumerable<OperationModel> Operations => this._hub.Operations.Select(o => new OperationModel(o.Key, o.Value, this._resolver));

        public IEnumerable<OperationModel> Callbacks => this._hub.Callbacks.Select(o => new OperationModel(o.Key, o.Value, this._resolver));
    }
}
