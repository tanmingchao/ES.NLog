NLog配置

<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
autoReload="true"
throwExceptions="false"
internalLogLevel="Off“
internalLogFile="c:\temp\nlog-internal.log">
<variable name="myvar" value="myvalue"/>
<targets> </targets>
<rules> </rules>
</nlog>

xmlns=“http://www.nlog-project.org/schemas/NLog.xsd” 这表示默认命名空间
xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 这个命名空间里面的元素或者属性就必须要以xsi:这种方式来写
比如schemaLocation就是他的一个属性，所以写成xsi:schemaLocation
而默认命名空间不带类似xsi这种，其实xml标签名称有个专业叫法叫做QName，而如果没有前面的xsi:这种一般叫做NCName
xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
表示把定义这个命名空间的schema文件给引用进来，好让开发类型工具能够解析和验证你的xml文件是否符合语法规范
等同于

简单来说 上面是用来验证你XML格式是否正确的。

InternalLogFile="c:\log\nlog.txt" //NLog内部日志文件位置 
internalLogLevel="Debug" //日志级别 
autoReload:一旦启动程序，这时候NLog.config文件被读取后，知道程序再启动都不会再读取配置文件了。假如我们不想停掉程序，比如说服务器哪能说停就停哈。这就用上这个配置了，这个配置功能是，一旦你对配置文件修改，程序将会重新读取配置文件，也就是自动再配置。

throwExceptions//NLog日志系统抛出异常
internalLogFile="c:\log\nlog.txt" //NLog内部日志文件位置 
internalLogLevel="Debug" //日志级别 

<variable /> - 定义配置文件中用到的变量
<targets /> - 定义日志的目标/输出
<rules /> - 定义日志的路由规则

 

Layout布局

几种常见的
${var:basePath} basePath是前面自定义的变量
${longdate} 日期格式 2017-01-17 16:58:03.8667
${shortdate}日期格式 2017-01-17 
${date:yyyyMMddHHmmssFFF} 日期 20170117165803866
${message} 输出内容
${guid} guid
${level}日志记录的等级
${logger} 配置的logger

NLog记录等级

Trace - 最常见的记录信息，一般用于普通输出
Debug - 同样是记录信息，不过出现的频率要比Trace少一些，一般用来调试程序
Info - 信息类型的消息
Warn - 警告信息，一般用于比较重要的场合
Error - 错误信息
Fatal - 致命异常信息。一般来讲，发生致命异常之后程序将无法继续执行。
自上而下，等级递增。

NLog等级使用

指定特定等级 如：level="Warn" 
指定多个等级 如：levels=“Warn,Debug“ 以逗号隔开
指定等级范围 如：minlevel="Warn" maxlevel="Error"

 

Logger使用

从配置文件读取信息并初始化 两种常用的方式

根据配置的路由名获生成特定的logger Logger logger = LogManager.GetLogger("LoggerDemo");

初始化为当前命名空间下当前类的logger  Logger logger = LogManager.GetCurrentClassLogger();

区别是logger的name不一样 前者是LoggerDemo，后者是当前命名空间+点+当前类名 如类比较多，并且往同一个日志文件记录，建议用GetCurrentClassLogger

 

Logger有以下三种常用的写入方式

logger.Error("这是DatabaseDemo的错误信息");
logger.Error(“ContentDemo {0}:{1}”,“时间”,DateTime.Now.ToString());需要拼接字符串的话推荐这种，NLog做了延迟处理，用的时候才拼接。
logger.Log(LogLevel.Error, "这是ContentDemo");

Logger发邮件参数

smtpServer=“*****” 邮件服务器 例如126邮箱是smtp.126.com
smtpPort=“25“端口
smtpAuthentication=“Basic“ 身份验证方式 基本
smtpUserName=“*****“ 邮件服务器用户名
smtpPassword=“******”邮件服务器密码
enableSsl=“false”是否使用安全连接 需要服务器支持
addNewLines=“true” 开头与结尾是否换行
from=“****” 发件邮箱
to=“XXXX@XX.com,XXXXX@XX.com”收件邮箱 多个以逗号分隔
subject=“subject:${machinename}报错“ 邮件主题
header=“---------------------开头-------------------------“ 邮件开头
body=“${newline}${message}${newline}“ 邮件内容
footer=“---------------------结尾-------------------------“ 邮件结尾




ABP 开源框架中配置参考
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="Warn"
      internalLogFile="App_Data\Logs\nlogs.txt">

  <variable name="logDirectory" value="${basedir}\log\"/>

  <!--define various log targets-->
  <targets>

    <!--write logs to file-->
    <target xsi:type="File" name="allfile" fileName="${logDirectory}\nlog-all-${shortdate}.log"
                 layout="${longdate}|${logger}|${uppercase:${level}}|${message} ${exception}" />

    <target xsi:type="File" name="ownFile-web" fileName="nlog-my-${shortdate}.log"
                 layout="${longdate}|${logger}|${uppercase:${level}}|${message} ${exception}" />

    <target xsi:type="Null" name="blackhole" />

  </targets>

  <rules>
    <!--All logs, including from Microsoft-->
    <logger name="*" minlevel="Trace" writeTo="allfile" />

    <!--Skip Microsoft logs and so log only own logs-->
    <logger name="Microsoft.*" minlevel="Trace" writeTo="blackhole" final="true" />
    <logger name="*" minlevel="Trace" writeTo="ownFile-web" />
  </rules>
</nlog>
