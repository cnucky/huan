using System;
using System.Data;
using System.Collections.Generic;
 using JiShi.Model;
namespace JiShi.BLL
{
	/// <summary>
	/// js_convert
	/// </summary>
	public partial class js_convert
	{
		private readonly JiShi.DAL.js_convert dal=new JiShi.DAL.js_convert();
		public js_convert()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int js_id)
		{
			return dal.Exists(js_id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(JiShi.Model.js_convert model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(JiShi.Model.js_convert model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int js_id)
		{
			
			return dal.Delete(js_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string js_idlist )
		{
			return dal.DeleteList(js_idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public JiShi.Model.js_convert GetModel(int js_id)
		{
			
			return dal.GetModel(js_id);
		}

        ///// <summary>
        ///// 得到一个对象实体，从缓存中
        ///// </summary>
        //public JiShi.Model.js_convert GetModelByCache(int js_id)
        //{
			
        //    string CacheKey = "js_convertModel-" + js_id;
        //    object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetModel(js_id);
        //            if (objModel != null)
        //            {
        //                int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
        //                Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch{}
        //    }
        //    return (JiShi.Model.js_convert)objModel;
        //}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<JiShi.Model.js_convert> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<JiShi.Model.js_convert> DataTableToList(DataTable dt)
		{
			List<JiShi.Model.js_convert> modelList = new List<JiShi.Model.js_convert>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				JiShi.Model.js_convert model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

