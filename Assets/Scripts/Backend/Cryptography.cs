using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class Cryptography
{
    public static string RequestSignatureKey;
    public static string ResponseSignatureKey;

    public static string GetSignature(MatchData matchData)
    {
        matchData.sign = string.Empty;
        return GetSignature(matchData, RequestSignatureKey);
    }

    public static string GetSignature(object data, string signatureKey)
    {
        string signature;
        byte[] unicodeKey = Encoding.UTF8.GetBytes(signatureKey);
        using (HMACSHA256 hmacSha256 = new HMACSHA256(unicodeKey))
        {
            string signString = JsonUtility.ToJson(data);
            byte[] dataToHmac = Encoding.UTF8.GetBytes(signString);
            signature = Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
        }

        return signature;
    }

    public static bool IsSignatureValid(PlayerStatus userStatus)
    {
        string incomingSignature = userStatus.sign;
        userStatus.sign = string.Empty;

        string ourSignature = GetSignature(userStatus, ResponseSignatureKey);
        
        return incomingSignature == ourSignature;
    }
}