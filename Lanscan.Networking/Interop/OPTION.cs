//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Interop
{
    public enum OPTION : uint
    {
        NAME = 102u,
        PAD = 0u,
        SUBNET_MASK = 1u,
        TIME_OFFSET = 2u,
        ROUTER_ADDRESS = 3u,
        TIME_SERVERS = 4u,
        IEN116_NAME_SERVERS = 5u,
        DOMAIN_NAME_SERVERS = 6u,
        LOG_SERVERS = 7u,
        COOKIE_SERVERS = 8u,
        LPR_SERVERS = 9u,
        IMPRESS_SERVERS = 10u,
        RLP_SERVERS = 11u,
        HOST_NAME = 12u,
        BOOT_FILE_SIZE = 13u,
        MERIT_DUMP_FILE = 14u,
        DOMAIN_NAME = 15u,
        SWAP_SERVER = 16u,
        ROOT_DISK = 17u,
        EXTENSIONS_PATH = 18u
    }
}
