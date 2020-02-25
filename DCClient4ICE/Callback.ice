//
// Copyright (c) ZeroC, Inc. All rights reserved.
//

#pragma once

["java:package:itc.common.ice"]
module dc
{
    sequence<byte> ByteSeq;
    interface SrvCallbackReceiver
    {
        ByteSeq callback(string extInfo,ByteSeq bytes);
    }
    interface SubCallbackReceiver
    {
        void callback(ByteSeq bytes);
    }

    interface DcClientIce
    {
        bool srv(string clientId,string topic,SrvCallbackReceiver* receiver);
        ByteSeq act(string topic,ByteSeq bytes);
        ByteSeq actDest(string dest,string topic,ByteSeq bytes);
       ["amd"] idempotent void pub(string topic,ByteSeq bytes);
       ["amd"] idempotent void pubDest(string dest,string topic,ByteSeq bytes);
        bool sub(string clientId,string topic,SubCallbackReceiver* receiver);
    }
}
