using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace JiShi.DAL
{
	/// <summary>
	/// 数据访问类:js_item
	/// </summary>
	public partial class js_item
	{
		public js_item()
		{}
		#region  Method


		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from js_item");
			strSql.Append(" where id='"+id+"' ");
			return DbHelperMySQL.Exists(strSql.ToString());
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(JiShi.Model.js_item model)
		{
			StringBuilder strSql=new StringBuilder();
			StringBuilder strSql1=new StringBuilder();
			StringBuilder strSql2=new StringBuilder();
			if (model.closed != null)
			{
				strSql1.Append("closed,");
				strSql2.Append("'"+model.closed+"',");
			}
			if (model.createDate != null)
			{
				strSql1.Append("createDate,");
				strSql2.Append("'"+model.createDate+"',");
			}
			if (model.curId != null)
			{
				strSql1.Append("curId,");
				strSql2.Append("'"+model.curId+"',");
			}
			if (model.deliveryDate != null)
			{
				strSql1.Append("deliveryDate,");
				strSql2.Append("'"+model.deliveryDate+"',");
			}
			if (model.gameId != null)
			{
				strSql1.Append("gameId,");
				strSql2.Append("'"+model.gameId+"',");
			}
			if (model.gameItemId != null)
			{
				strSql1.Append("gameItemId,");
				strSql2.Append("'"+model.gameItemId+"',");
			}
			if (model.gender != null)
			{
				strSql1.Append("gender,");
				strSql2.Append("'"+model.gender+"',");
			}
			if (model.grade != null)
			{
				strSql1.Append("grade,");
				strSql2.Append("'"+model.grade+"',");
			}
			if (model.gradeName != null)
			{
				strSql1.Append("gradeName,");
				strSql2.Append("'"+model.gradeName+"',");
			}
			if (model.guild != null)
			{
				strSql1.Append("guild,");
				strSql2.Append("'"+model.guild+"',");
			}
			if (model.iconPath != null)
			{
				strSql1.Append("iconPath,");
				strSql2.Append("'"+model.iconPath+"',");
			}
			if (model.id != null)
			{
				strSql1.Append("id,");
				strSql2.Append("'"+model.id+"',");
			}
			if (model.itemAmount != null)
			{
				strSql1.Append("itemAmount,");
				strSql2.Append("'"+model.itemAmount+"',");
			}
			if (model.itemCode != null)
			{
				strSql1.Append("itemCode,");
				strSql2.Append("'"+model.itemCode+"',");
			}
			if (model.itemDesc != null)
			{
				strSql1.Append("itemDesc,");
				strSql2.Append("'"+model.itemDesc+"',");
			}
			if (model.itemName != null)
			{
				strSql1.Append("itemName,");
				strSql2.Append("'"+model.itemName+"',");
			}
			if (model.itemType != null)
			{
				strSql1.Append("itemType,");
				strSql2.Append("'"+model.itemType+"',");
			}
			if (model.power != null)
			{
				strSql1.Append("power,");
				strSql2.Append("'"+model.power+"',");
			}
			if (model.price != null)
			{
				strSql1.Append("price,");
				strSql2.Append("'"+model.price+"',");
			}
			if (model.publicityEndDate != null)
			{
				strSql1.Append("publicityEndDate,");
				strSql2.Append("'"+model.publicityEndDate+"',");
			}
			if (model.returnDate != null)
			{
				strSql1.Append("returnDate,");
				strSql2.Append("'"+model.returnDate+"',");
			}
			if (model.saveTime != null)
			{
				strSql1.Append("saveTime,");
				strSql2.Append("'"+model.saveTime+"',");
			}
			if (model.sellerCasId != null)
			{
				strSql1.Append("sellerCasId,");
				strSql2.Append("'"+model.sellerCasId+"',");
			}
			if (model.sellerGameId != null)
			{
				strSql1.Append("sellerGameId,");
				strSql2.Append("'"+model.sellerGameId+"',");
			}
			if (model.sellerRole != null)
			{
				strSql1.Append("sellerRole,");
				strSql2.Append("'"+model.sellerRole+"',");
			}
			if (model.serverId != null)
			{
				strSql1.Append("serverId,");
				strSql2.Append("'"+model.serverId+"',");
			}
			if (model.shelfDate != null)
			{
				strSql1.Append("shelfDate,");
				strSql2.Append("'"+model.shelfDate+"',");
			}
			if (model.shelfDays != null)
			{
				strSql1.Append("shelfDays,");
				strSql2.Append("'"+model.shelfDays+"',");
			}
			if (model.status != null)
			{
				strSql1.Append("status,");
				strSql2.Append("'"+model.status+"',");
			}
			if (model.unitPrice != null)
			{
				strSql1.Append("unitPrice,");
				strSql2.Append("'"+model.unitPrice+"',");
			}
            //if (model.temp1 != null)
            //{
            //    strSql1.Append("temp1,");
            //    strSql2.Append("'"+model.temp1+"',");
            //}
            //if (model.temp2 != null)
            //{
            //    strSql1.Append("temp2,");
            //    strSql2.Append("'"+model.temp2+"',");
            //}
            //if (model.temp3 != null)
            //{
            //    strSql1.Append("temp3,");
            //    strSql2.Append("'"+model.temp3+"',");
            //}
            //if (model.temp4 != null)
            //{
            //    strSql1.Append("temp4,");
            //    strSql2.Append("'"+model.temp4+"',");
            //}
            //if (model.temp5 != null)
            //{
            //    strSql1.Append("temp5,");
            //    strSql2.Append("'"+model.temp5+"',");
            //}
            //if (model.temp6 != null)
            //{
            //    strSql1.Append("temp6,");
            //    strSql2.Append("'"+model.temp6+"',");
            //}
            //if (model.temp7 != null)
            //{
            //    strSql1.Append("temp7,");
            //    strSql2.Append("'"+model.temp7+"',");
            //}
			strSql.Append("insert into js_item(");
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
		public bool Update(JiShi.Model.js_item model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update js_item set ");
			if (model.closed != null)
			{
				strSql.Append("closed='"+model.closed+"',");
			}
			else
			{
				strSql.Append("closed= null ,");
			}
			if (model.createDate != null)
			{
				strSql.Append("createDate='"+model.createDate+"',");
			}
			else
			{
				strSql.Append("createDate= null ,");
			}
			if (model.curId != null)
			{
				strSql.Append("curId='"+model.curId+"',");
			}
			else
			{
				strSql.Append("curId= null ,");
			}
			if (model.deliveryDate != null)
			{
				strSql.Append("deliveryDate='"+model.deliveryDate+"',");
			}
			else
			{
				strSql.Append("deliveryDate= null ,");
			}
			if (model.gameId != null)
			{
				strSql.Append("gameId='"+model.gameId+"',");
			}
			else
			{
				strSql.Append("gameId= null ,");
			}
			if (model.gameItemId != null)
			{
				strSql.Append("gameItemId='"+model.gameItemId+"',");
			}
			else
			{
				strSql.Append("gameItemId= null ,");
			}
			if (model.gender != null)
			{
				strSql.Append("gender='"+model.gender+"',");
			}
			else
			{
				strSql.Append("gender= null ,");
			}
			if (model.grade != null)
			{
				strSql.Append("grade='"+model.grade+"',");
			}
			else
			{
				strSql.Append("grade= null ,");
			}
			if (model.gradeName != null)
			{
				strSql.Append("gradeName='"+model.gradeName+"',");
			}
			else
			{
				strSql.Append("gradeName= null ,");
			}
			if (model.guild != null)
			{
				strSql.Append("guild='"+model.guild+"',");
			}
			else
			{
				strSql.Append("guild= null ,");
			}
			if (model.iconPath != null)
			{
				strSql.Append("iconPath='"+model.iconPath+"',");
			}
			else
			{
				strSql.Append("iconPath= null ,");
			}
			if (model.itemAmount != null)
			{
				strSql.Append("itemAmount='"+model.itemAmount+"',");
			}
			else
			{
				strSql.Append("itemAmount= null ,");
			}
			if (model.itemCode != null)
			{
				strSql.Append("itemCode='"+model.itemCode+"',");
			}
			else
			{
				strSql.Append("itemCode= null ,");
			}
			if (model.itemDesc != null)
			{
				strSql.Append("itemDesc='"+model.itemDesc+"',");
			}
			else
			{
				strSql.Append("itemDesc= null ,");
			}
			if (model.itemName != null)
			{
				strSql.Append("itemName='"+model.itemName+"',");
			}
			else
			{
				strSql.Append("itemName= null ,");
			}
			if (model.itemType != null)
			{
				strSql.Append("itemType='"+model.itemType+"',");
			}
			else
			{
				strSql.Append("itemType= null ,");
			}
			if (model.power != null)
			{
				strSql.Append("power='"+model.power+"',");
			}
			else
			{
				strSql.Append("power= null ,");
			}
			if (model.price != null)
			{
				strSql.Append("price='"+model.price+"',");
			}
			else
			{
				strSql.Append("price= null ,");
			}
			if (model.publicityEndDate != null)
			{
				strSql.Append("publicityEndDate='"+model.publicityEndDate+"',");
			}
			else
			{
				strSql.Append("publicityEndDate= null ,");
			}
			if (model.returnDate != null)
			{
				strSql.Append("returnDate='"+model.returnDate+"',");
			}
			else
			{
				strSql.Append("returnDate= null ,");
			}
			if (model.saveTime != null)
			{
				strSql.Append("saveTime='"+model.saveTime+"',");
			}
			else
			{
				strSql.Append("saveTime= null ,");
			}
			if (model.sellerCasId != null)
			{
				strSql.Append("sellerCasId='"+model.sellerCasId+"',");
			}
			else
			{
				strSql.Append("sellerCasId= null ,");
			}
			if (model.sellerGameId != null)
			{
				strSql.Append("sellerGameId='"+model.sellerGameId+"',");
			}
			else
			{
				strSql.Append("sellerGameId= null ,");
			}
			if (model.sellerRole != null)
			{
				strSql.Append("sellerRole='"+model.sellerRole+"',");
			}
			else
			{
				strSql.Append("sellerRole= null ,");
			}
			if (model.serverId != null)
			{
				strSql.Append("serverId='"+model.serverId+"',");
			}
			else
			{
				strSql.Append("serverId= null ,");
			}
			if (model.shelfDate != null)
			{
				strSql.Append("shelfDate='"+model.shelfDate+"',");
			}
			else
			{
				strSql.Append("shelfDate= null ,");
			}
			if (model.shelfDays != null)
			{
				strSql.Append("shelfDays='"+model.shelfDays+"',");
			}
			else
			{
				strSql.Append("shelfDays= null ,");
			}
			if (model.status != null)
			{
				strSql.Append("status='"+model.status+"',");
			}
			else
			{
				strSql.Append("status= null ,");
			}
			if (model.unitPrice != null)
			{
				strSql.Append("unitPrice='"+model.unitPrice+"',");
			}
			else
			{
				strSql.Append("unitPrice= null ,");
			}
            //if (model.temp1 != null)
            //{
            //    strSql.Append("temp1='"+model.temp1+"',");
            //}
            //else
            //{
            //    strSql.Append("temp1= null ,");
            //}
            //if (model.temp2 != null)
            //{
            //    strSql.Append("temp2='"+model.temp2+"',");
            //}
            //else
            //{
            //    strSql.Append("temp2= null ,");
            //}
            //if (model.temp3 != null)
            //{
            //    strSql.Append("temp3='"+model.temp3+"',");
            //}
            //else
            //{
            //    strSql.Append("temp3= null ,");
            //}
            //if (model.temp4 != null)
            //{
            //    strSql.Append("temp4='"+model.temp4+"',");
            //}
            //else
            //{
            //    strSql.Append("temp4= null ,");
            //}
            //if (model.temp5 != null)
            //{
            //    strSql.Append("temp5='"+model.temp5+"',");
            //}
            //else
            //{
            //    strSql.Append("temp5= null ,");
            //}
            //if (model.temp6 != null)
            //{
            //    strSql.Append("temp6='"+model.temp6+"',");
            //}
            //else
            //{
            //    strSql.Append("temp6= null ,");
            //}
            //if (model.temp7 != null)
            //{
            //    strSql.Append("temp7='"+model.temp7+"',");
            //}
            //else
            //{
            //    strSql.Append("temp7= null ,");
            //}
			int n = strSql.ToString().LastIndexOf(",");
			strSql.Remove(n, 1);
			strSql.Append(" where id='"+ model.id+"' ");
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
		public bool Delete(string id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from js_item ");
			strSql.Append(" where id='"+id+"' " );
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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from js_item ");
			strSql.Append(" where id in ("+idlist + ")  ");
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
		public JiShi.Model.js_item GetModel(string id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  ");
			strSql.Append(" closed,createDate,curId,deliveryDate,gameId,gameItemId,gender,grade,gradeName,guild,iconPath,id,itemAmount,itemCode,itemDesc,itemName,itemType,power,price,publicityEndDate,returnDate,saveTime,sellerCasId,sellerGameId,sellerRole,serverId,shelfDate,shelfDays,status,unitPrice,temp1,temp2,temp3,temp4,temp5,temp6,temp7 ");
			strSql.Append(" from js_item ");
			strSql.Append(" where id='"+id+"' " );
			JiShi.Model.js_item model=new JiShi.Model.js_item();
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
		public JiShi.Model.js_item DataRowToModel(DataRow row)
		{
			JiShi.Model.js_item model=new JiShi.Model.js_item();
			if (row != null)
			{
				if(row["closed"]!=null)
				{
					model.closed=row["closed"].ToString();
				}
				if(row["createDate"]!=null)
				{
					model.createDate=row["createDate"].ToString();
				}
				if(row["curId"]!=null)
				{
					model.curId=row["curId"].ToString();
				}
				if(row["deliveryDate"]!=null)
				{
					model.deliveryDate=row["deliveryDate"].ToString();
				}
				if(row["gameId"]!=null)
				{
					model.gameId=row["gameId"].ToString();
				}
				if(row["gameItemId"]!=null)
				{
					model.gameItemId=row["gameItemId"].ToString();
				}
				if(row["gender"]!=null)
				{
					model.gender=row["gender"].ToString();
				}
				if(row["grade"]!=null)
				{
					model.grade=row["grade"].ToString();
				}
				if(row["gradeName"]!=null)
				{
					model.gradeName=row["gradeName"].ToString();
				}
				if(row["guild"]!=null)
				{
					model.guild=row["guild"].ToString();
				}
				if(row["iconPath"]!=null)
				{
					model.iconPath=row["iconPath"].ToString();
				}
				if(row["id"]!=null)
				{
					model.id=row["id"].ToString();
				}
				if(row["itemAmount"]!=null)
				{
					model.itemAmount=row["itemAmount"].ToString();
				}
				if(row["itemCode"]!=null)
				{
					model.itemCode=row["itemCode"].ToString();
				}
				if(row["itemDesc"]!=null)
				{
					model.itemDesc=row["itemDesc"].ToString();
				}
				if(row["itemName"]!=null)
				{
					model.itemName=row["itemName"].ToString();
				}
				if(row["itemType"]!=null)
				{
					model.itemType=row["itemType"].ToString();
				}
				if(row["power"]!=null)
				{
					model.power=row["power"].ToString();
				}
				if(row["price"]!=null)
				{
					model.price=row["price"].ToString();
				}
				if(row["publicityEndDate"]!=null)
				{
					model.publicityEndDate=row["publicityEndDate"].ToString();
				}
				if(row["returnDate"]!=null)
				{
					model.returnDate=row["returnDate"].ToString();
				}
				if(row["saveTime"]!=null)
				{
					model.saveTime=row["saveTime"].ToString();
				}
				if(row["sellerCasId"]!=null)
				{
					model.sellerCasId=row["sellerCasId"].ToString();
				}
				if(row["sellerGameId"]!=null)
				{
					model.sellerGameId=row["sellerGameId"].ToString();
				}
				if(row["sellerRole"]!=null)
				{
					model.sellerRole=row["sellerRole"].ToString();
				}
				if(row["serverId"]!=null)
				{
					model.serverId=row["serverId"].ToString();
				}
				if(row["shelfDate"]!=null)
				{
					model.shelfDate=row["shelfDate"].ToString();
				}
				if(row["shelfDays"]!=null)
				{
					model.shelfDays=row["shelfDays"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["unitPrice"]!=null)
				{
					model.unitPrice=row["unitPrice"].ToString();
				}
                //if(row["temp1"]!=null)
                //{
                //    model.temp1=row["temp1"].ToString();
                //}
                //if(row["temp2"]!=null)
                //{
                //    model.temp2=row["temp2"].ToString();
                //}
                //if(row["temp3"]!=null)
                //{
                //    model.temp3=row["temp3"].ToString();
                //}
                //if(row["temp4"]!=null)
                //{
                //    model.temp4=row["temp4"].ToString();
                //}
                //if(row["temp5"]!=null)
                //{
                //    model.temp5=row["temp5"].ToString();
                //}
                //if(row["temp6"]!=null)
                //{
                //    model.temp6=row["temp6"].ToString();
                //}
                //if(row["temp7"]!=null)
                //{
                //    model.temp7=row["temp7"].ToString();
                //}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select closed,createDate,curId,deliveryDate,gameId,gameItemId,gender,grade,gradeName,guild,iconPath,id,itemAmount,itemCode,itemDesc,itemName,itemType,power,price,publicityEndDate,returnDate,saveTime,sellerCasId,sellerGameId,sellerRole,serverId,shelfDate,shelfDays,status,unitPrice,temp1,temp2,temp3,temp4,temp5,temp6,temp7 ");
			strSql.Append(" FROM js_item ");
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
			strSql.Append("select count(1) FROM js_item ");
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
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from js_item T ");
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

