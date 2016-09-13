using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace JiShi.DAL
{
	/// <summary>
	/// 数据访问类:js_convert
	/// </summary>
	public partial class js_convert
	{
		public js_convert()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("js_id", "js_convert"); 
		}


		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int js_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from js_convert");
			strSql.Append(" where js_id="+js_id+" ");
			return DbHelperMySQL.Exists(strSql.ToString());
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(JiShi.Model.js_convert model)
		{
			StringBuilder strSql=new StringBuilder();
			StringBuilder strSql1=new StringBuilder();
			StringBuilder strSql2=new StringBuilder();
			if (model.js_id != null)
			{
				strSql1.Append("js_id,");
				strSql2.Append(""+model.js_id+",");
			}
			if (model.js_teype != null)
			{
				strSql1.Append("js_teype,");
				strSql2.Append("'"+model.js_teype+"',");
			}
			if (model.js_source != null)
			{
				strSql1.Append("js_source,");
				strSql2.Append("'"+model.js_source+"',");
			}
			if (model.js_dest != null)
			{
				strSql1.Append("js_dest,");
				strSql2.Append("'"+model.js_dest+"',");
			}
			strSql.Append("insert into js_convert(");
			strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
			strSql.Append(")");
			strSql.Append(" values (");
			strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
			strSql.Append(")");
			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(JiShi.Model.js_convert model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update js_convert set ");
			if (model.js_teype != null)
			{
				strSql.Append("js_teype='"+model.js_teype+"',");
			}
			else
			{
				strSql.Append("js_teype= null ,");
			}
			if (model.js_source != null)
			{
				strSql.Append("js_source='"+model.js_source+"',");
			}
			else
			{
				strSql.Append("js_source= null ,");
			}
			if (model.js_dest != null)
			{
				strSql.Append("js_dest='"+model.js_dest+"',");
			}
			else
			{
				strSql.Append("js_dest= null ,");
			}
			int n = strSql.ToString().LastIndexOf(",");
			strSql.Remove(n, 1);
			strSql.Append(" where js_id="+ model.js_id+" ");
			int rowsAffected=DbHelperMySQL.ExecuteSql(strSql.ToString());
			if (rowsAffected > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int js_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from js_convert ");
			strSql.Append(" where js_id="+js_id+" " );
			int rowsAffected=DbHelperMySQL.ExecuteSql(strSql.ToString());
			if (rowsAffected > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string js_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from js_convert ");
			strSql.Append(" where js_id in ("+js_idlist + ")  ");
			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public JiShi.Model.js_convert GetModel(int js_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  ");
			strSql.Append(" js_id,js_teype,js_source,js_dest ");
			strSql.Append(" from js_convert ");
			strSql.Append(" where js_id="+js_id+" " );
			JiShi.Model.js_convert model=new JiShi.Model.js_convert();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString());
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public JiShi.Model.js_convert DataRowToModel(DataRow row)
		{
			JiShi.Model.js_convert model=new JiShi.Model.js_convert();
			if (row != null)
			{
				if(row["js_id"]!=null && row["js_id"].ToString()!="")
				{
					model.js_id=int.Parse(row["js_id"].ToString());
				}
				if(row["js_teype"]!=null)
				{
					model.js_teype=row["js_teype"].ToString();
				}
				if(row["js_source"]!=null)
				{
					model.js_source=row["js_source"].ToString();
				}
				if(row["js_dest"]!=null)
				{
					model.js_dest=row["js_dest"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select js_id,js_teype,js_source,js_dest ");
			strSql.Append(" FROM js_convert ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM js_convert ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.js_id desc");
			}
			strSql.Append(")AS Row, T.*  from js_convert T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/*
		*/

		#endregion  Method
		#region  MethodEx

		#endregion  MethodEx
	}
}

