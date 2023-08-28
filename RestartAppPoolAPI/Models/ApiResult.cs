using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestartAppPoolAPI.Models
{
    public class ApiResult<T>
    {
        /// <summary>
        /// 執行成功與否
        /// </summary>
        public bool Succ { get; set; }
        /// <summary>
        /// 結果代碼(0000=成功，其餘為錯誤代號)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 資料時間
        /// </summary>
        public DateTime DataTime { get; set; }
        /// <summary>
        /// 資料本體
        /// </summary>
        public T Data { get; set; }


        public ApiResult() { }

        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(T data, string code, bool succ)
        {
            Code = code;
            Succ = succ;
            DataTime = DateTime.Now;
            Data = data;
        }
        
    }
}