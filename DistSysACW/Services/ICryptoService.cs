using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public interface ICryptoService
    {
        byte[] Decrypt(byte[] data);
        string GetPublicKey();
    }
}
