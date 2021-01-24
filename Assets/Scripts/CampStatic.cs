using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampStatic 
{
    public static bool CompareCamp(Entity self,Entity other)
    {
        if (self.Camp == other.Camp)
            return true;
        if (self.Camp == "Main")
        {
            if (other.Camp == "Enemy")
            return true;
        }
        return false;
    }
}
