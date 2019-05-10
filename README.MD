简单的多数据源适配方案
---
#### 有以下几种方式进行数据源的适配
##### 1.使用配置文件的方式,在项目的app.config文件中已有相关配置的示例,请查看
调用方式如:<br>
 
 > ///使用app.config文件中的配置进行初始化<br>
 DbContext context = new DbContext("SqlClientProvide", DataBaseType.None);<br>
 <br>
 
 > ///或者<br>
 DbContext context = 
 new DbContext(new DbHelper.ConnectionStrBuilder.SqlServerConStrBuilder(".", "Demo"), DataBaseType.None,"SqlClientProvide");
 <br>
 
  > ///此种方式需要您引用相关数据源的dll,并在程序app.config文件中进行相关的配置
 
##### 2.使用内置的数据源
调用方式如:<br>
 >  DbContext context = new DbContext("Data Source=.;Initial Catalog=Temp;Integrated Security=True", DataBaseType.SqlServer);
 
#### 其它使用方式请查看项目中的SimpleDemo,该示例使用内置的PostgreSql内置数据源

> 另外,请注意,该项目使用了 'Fody' ,请使用Visual Studio 2017 打开本项目解决方案,否则会编译不通过
  如果需要使用低版本Visual Studio打开项目,请卸载项目中的 'Fody'
