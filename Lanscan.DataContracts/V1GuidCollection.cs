//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [CollectionDataContract(Namespace = V1Constants.EmptyNamespace, ItemName = V1Constants.Guid)]
    public sealed class V1GuidCollection : List<Guid>
    {
        public V1GuidCollection()
        {
        }
    }
}
