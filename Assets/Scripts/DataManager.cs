using UnityEngine;

namespace MJ
{
    using MJ.Data;

    public static class DataManager
    {
        public static N_BackData N_BackData => n_BackData;
        private readonly static N_BackData n_BackData = new N_BackData();


    }
}