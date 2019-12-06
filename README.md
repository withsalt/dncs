# 毒鸡汤
#### 介绍
基于egotong的项目[nows](https://github.com/egotong/nows "nows")，~~在nows的基础上~~，应该是除了前端界面外，其余全部替换为ASP.NET Core。后端采用ASP.NET Core，数据库访问方式为EF Core，数据库采用PostgreSQL。  
其实这么大费周折做的原因是基于nows开发了一个微信公众号，但是无奈不会php，只有用ASP.NET Core全部重新构建一遍，但是无奈于微信部分的代码涉及到很多有关于安全性方面的问题，而且都是业务逻辑代码，所以抱歉微信部分代码不能开源。  

#### 安装教程
1、Clone项目
2、首先创建postgresql的数据库用户和数据库访问权限。数据库EF会自动创建，所以只需要配置对应数据库用户权限即可。然后配置appsettings.json中的数据库链接字符串。  
3、编译并运行

#### 其它
还是讨要一波关注，公众号每天都会定时推送。

[![二维码](https://github.com/withsalt/dncs/blob/master/document/qrcode.jpg "二维码")](https://github.com/withsalt/dncs/blob/master/document/qrcode.jpg "二维码")
