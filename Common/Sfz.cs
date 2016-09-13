using System;
using System.Text;
using NUnit.Framework;
namespace Common {
    /// <summary> 
    /// 为性别提供枚举 
    /// </summary> 
    public enum Sex {
        /// <summary> 
        /// 男性 
        /// </summary> 
        Male = 1,
        /// <summary> 
        /// 女性 
        /// </summary> 
        Female = 2,
    }

    public class ShenFenZhengExpection : Exception {
         
        public ShenFenZhengExpection(string msg) : base(msg) { }
    }

    /// <summary> 
    /// 中华人民共和国身份证的结构,仅支持18位身份证 
    /// </summary> 
    [TestFixture]
    public class ShenFenZheng {
        public ShenFenZheng() {

        }

        [Test]
        public void RadomProduceId() {
            for (int i = 0; i < 50; i++) {
                Console.WriteLine(ProduceShenFenZheng().number);
            }
            


        }

        string _Id;

        /// <summary> 
        /// 创建一个新的身份证的结构 
        /// </summary> 
        /// <param name="code">地区代码</param> 
        /// <param name="birthDay">birthday</param> 
        /// <param name="orderID">顺序码</param> 
        public ShenFenZheng(uint code, DateTime birthDay, ushort orderID) {
            _Id = (GetFrontCode(code, birthDay, orderID) + GetVcode(code, birthDay, orderID));
        }

        /// <summary> 
        /// 创建一个新的身份证的结构 
        /// </summary> 
        /// <param name="id">身份证号码</param> 
        /// <exception cref="ShenFenZhengExpection">验证码错误时抛出</exception> 
        public ShenFenZheng(string id) {
            if (IsPass(id))
                _Id = id;
            else
                throw new ShenFenZhengExpection("验证码错误");
        }

        /// <summary> 
        /// 获得性别 
        /// </summary> 
        /// <value>sex</value> 
        public Sex sex {
            get {
                //*以顺序号为准.单数为男性,双数为女性 
                int s;
                Math.DivRem(Int32.Parse(number.Substring(14, 3)), 2, out s);
                if (System.Convert.ToBoolean(s))
                    return Sex.Male;
                else
                    return Sex.Female;
            }
        }

        /// <summary> 
        /// 获得生日 
        /// </summary> 
        /// <value>birthday</value> 
        public DateTime birthday {
            get {
                return new DateTime(
                    Int32.Parse(number.Substring(6, 4)),
                    Int32.Parse(number.Substring(8, 2)),
                    Int32.Parse(number.Substring(10, 2)));
            }
        }

        /// <summary> 
        /// 获得身份证号 
        /// </summary> 
        /// <value>身份证号</value> 
        public string number {
            get { return _Id; }
        }

        /// <summary> 
        /// GetVcode,长度为1位 
        /// </summary> 
        /// <value>Vcode</value> 
        public string Vcode {
            get { return number.Substring(17, 1); }
        }

        //* 要知道地区代码所代表的地区,请查阅"中国地区代码" 
        /// <summary> 
        /// 获得地址代码,长度为6位 
        /// </summary> 
        /// <value>AddressCode</value> 
        public uint AddressCode {
            get { return uint.Parse(number.Substring(0, 6)); }
        }

        /// <summary> 
        /// 获得顺序号,长度为3位 
        /// </summary> 
        /// <value>ShunXuhao</value> 
        public ushort ShunXuhao {
            get { return ushort.Parse(number.Substring(14, 3)); }
        }

        /// <summary> 
        /// 将身份证号转换为字串符的形式 
        /// </summary> 
        /// <returns>身份证号</returns> 
        public override string ToString() {
            return number;
        }

        /// <summary> 
        /// 检测身份证号是否正确,如果为非法的身份证号,则会抛出异常 
        /// </summary> 
        /// <param name="id">身份证号</param> 
        /// <returns>正确返回 True</returns> 
        /// <exception cref="ShenFenZhengExpection">身份证号位数不足或包含非法的字符抛出异常</exception> 
        public static bool IsPass(string id) {
            if (id.Length != 18)
                throw new ShenFenZhengExpection("身份证号位数不足");
            if (System.Text.RegularExpressions.Regex.IsMatch(id, @"^[0-9]{17}[0-9|X|x]$"))
                throw new ShenFenZhengExpection("身份证号中包含非法的字符");
            if (id.Substring(17, 1) != GetVcode(id))
                return false;
            return true;
        }

        /// <summary> 
        /// 获得身份证的验证码 
        /// </summary> 
        /// <param name="code">地区码</param> 
        /// <param name="birthDay">birthday</param> 
        /// <param name="orderID">顺序码</param> 
        /// <returns>Vcode</returns> 
        public static string GetVcode(uint code, DateTime birthDay, ushort orderID) {
            return GetVcode(GetFrontCode(code, birthDay, orderID));
        }

        /// <summary> 
        /// 获得身份证的验证码,参数为身份证的前缀17位 
        /// </summary> 
        /// <param name="front">前缀17位</param> 
        /// <returns>Vcode</returns> 
        public static string GetVcode(string front) {
            //*效验码求法：    所有前17位数字的对应码乘该数字的值的和, 
            //*                并求这个和的余数与效验码对照表对照则得到正确的效验码 
            byte[] cStr = new byte[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //*效验码对照表 0-10 : 1,0,X,9,8,7,6,5,4,3,2 
            string vStr = "10X98765432";

            //*计数器 
            int count = 0;

            //*求和 
            for (int i = 0; i < cStr.Length; i++) {
                count += cStr[i] * System.Convert.ToInt32(front.Substring(i, 1));
            }
            //*求余 
            int reVal;
            Math.DivRem(count, 11, out reVal);
            return vStr.Substring(reVal, 1);
        }

        private static string GetFrontCode(uint code, DateTime birthDay, ushort orderID) {
            //* Id = 地区码 + birthday + 顺序码 + Vcode 
            return code.ToString("000000") + birthDay.ToString("yyyyMMdd") + orderID.ToString("000");
        }



        /// <summary> 
        /// 生成一个随机身份证 
        /// </summary> 
        /// <returns>一个身份证</returns> 
        public static ShenFenZheng ProduceShenFenZheng() {
            uint code = uint.Parse(RandomManager.RandomReadOneLine("config//shenfenzheng1.txt"));
            DateTime birthDay = new DateTime(new Random().Next(1970, 1994),
                new Random().Next(1, 12), new Random().Next(1, 28));
            ushort orderID = (ushort)new Random().Next(0, 999);
            return new ShenFenZheng(code, birthDay, orderID);
        }


        #region =========quyuids=========
        private int[] quyuids = {
            110101,
            110102,
            110103,
            110104,
            110105,
            110106,
            110107,
            110108,
            110109,
            130101,
            130102,
            130103,
            140101,
            140302,
            150101,
            150428,
            210101,
            210204,
            220101,
            220105,
            230102,
            310109,
            320102,
            330783,
            340102,
            350104,
            360103,
            370103,
            370203,
            410102,
            420105,
            430102,
            440104,
            450104,
            460028,
            510106,
            520112,
            530103,
            610102,
            620103
        };
        #endregion
        private bool CheckIDCard18(string Id) {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false) {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1) {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false) {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++) {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower()) {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }



    }
}
/*
 <option value="110101">北京市东城区</option>
<option value="110102">北京市西城区</option>
<option value="110103">北京市崇文区</option>
<option value="110104">北京市宣武区</option>
<option value="110105">北京市朝阳区</option>
<option value="110106">北京市丰台区</option>
<option value="110107">北京市石景山区</option>
<option value="110108">北京市海淀区</option>
<option value="110109">北京市门头沟区</option>

<option value="130101">河北省石家庄市市辖区</option>
<option value="130102">河北省石家庄市长安区</option>
<option value="130103">河北省石家庄市桥东区</option>
<option value="140101">山西省太原市市辖区</option>
<option value="140302">山西省阳泉市城区</option>
<option value="150101">内蒙古呼和浩特市市辖区</option>
<option value="150428">内蒙古喀喇沁旗</option>
<option value="210101">辽宁省沈阳市市辖区</option>
<option value="210204">辽宁省大连市沙河口区</option>
<option value="220101">吉林省长春市市辖区</option>
<option value="220105">吉林省长春市二道河子区</option>
<option value="230102">黑龙江哈尔滨市道里区</option>
<option value="310109">上海市虹口区</option>
<option value="320102">江苏省南京市玄武区</option>
<option value="330783">浙江省东阳市</option>
<option value="340102">安徽省合肥市东市区</option>
<option value="350104">福建省福州市仓山区</option>
<option value="360103">江西省南昌市西湖区</option>
<option value="370103">山东省济南市市中区</option>
<option value="370203">山东省青岛市市北区</option>
<option value="410102">河南省郑州市中原区</option>
<option value="420105">湖北省武汉市汉阳区</option>
<option value="430102">湖南省长沙市东区</option>
<option value="440104">广东省广州市越秀区</option>
<option value="450104">广西南宁市城北区</option>
<option value="460028">海南省临高县</option>
<option value="510106">四川省成都市金牛区</option>
<option value="520112">贵州省贵阳市乌当区</option>
<option value="530103">云南省昆明市盘龙区</option>
<option value="540102">西藏拉萨市城关区</option>
<option value="610102">陕西省西安市新城区</option>
<option value="620103">甘肃省兰州市七里河区</option>
 
 */