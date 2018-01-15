// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System.Collections.Generic;

namespace gw.gql
{
    public partial class Query
    {
        public enum Action
        {
            Get,
            Set,
            Invoke,
        }

        public object           Root        { get; private set; }
        public Path             SearchPath  { get; private set; }

        public List<object>     Selected    { get; private set; }


        public Query( string q, object root )
        {
            Root        = root;
            SearchPath  = new Path( q );
            Selected    = new List<object>();
        }

        public void Clear()
        {
            Selected.Clear();
        }

        public List<object> Execute()
        {
            switch( SearchPath.Action )
            {
                case Query.Action.Get:      return Selected;
                case Query.Action.Set:      return ActionSet();
                case Query.Action.Invoke:   return ActionInvoke();
            }

            return null;
        }
    }
}

#endif
