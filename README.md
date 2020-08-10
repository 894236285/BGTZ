## 简介

本应用基于[Native.SDK](https://github.com/Jie2GG/Native.Framework)，主要功能是获取起点中文网的书籍更新情况，如章节名称、更新时间、字数。

## 特点

* 同时获取多本书的更新情况
* 同时发送到多个QQ群
* 要获取的书籍和发送消息的QQ群可配置
* 即时发送QQ消息

## 实现思路
* 章节报更：在应用启动时，使用System.Timer的计时器，每隔一定时间从起点服务器获取书籍的章节列表，然后筛选出最新的章节，并发布到群内。(可配置书籍名称和要发布的群)
* 入群欢迎：在新成员新增事件中，发送群消息并@新群员。(可配置启用入群欢迎的群和欢迎语及是否发送图片)

## 配置文件

1. 章节报更涉及2个文件配置，分别为Book.xml,xxx.json
* Book.xml示例格式及内容，其中id是起点中文网的书籍id,code和name是自定义命名，code会根据名称生成最新章节的json，name一般用书籍名称易于理解阅读，group节点是获取的章节信息将要发到哪些群，如有多本书则添加多个book节点
```xml
<?xml version="1.0" encoding="utf-8" ?>
 <root>
    <books>
		<book id="1015358161" code="xhdz" name="玄浑道章">
			<group>221827649</group>
			<group>615387042</group>
			<group>762873632</group>
			<group>526275426</group>
		</book>		
	</books>
</root>
```

* xxx.json是book.xml的code为名，如上例，这个json就名为xhdz.json，存储的是已获取的最新章节，用来与服务器的章节信息比较，判断是否是最新章节
> {"ChapterName":"第二十七章 昙光","ChapterTime":"2020-03-16 18:13:54","WordNumber":3136}
---
2. 入群欢迎涉及1个文件配置，即Group.xml,存储的是哪些群启用了入群欢迎,group节点的内容存储的是群号，而它的属性则代表了一些其他配置，text:欢迎语;isSendImage:是否发送图片,取值是Y和N,imageName则是图片文件的名称(带后缀),如果不需要为空就好,属性名还是要,不写属性可能会报错(没试过),酷Q图片地址是\data\image;mirai图片存放地址是\jre\lib\images,如有多个群则添加多个group节点
```xml 
<?xml version="1.0" encoding="utf-8" ?>
<root>
    <groups>
		<group text=" 欢迎新道友加入,本群是玄浑道章V群,需要验证粉丝值,请发送带有ID的粉丝值截图在群里,谢谢！ 如何查看带有ID的粉丝值截图如下：" isSendImage="Y" imageName="fensizhi.png">615387042</group>
		<group text="测试测试" isSendImage="N" imageName="">558628009</group>
	</groups>
</root>
```

## 说明
1. 代码中关于配置文件的加载获取都是取的相对路径，机器人data目录下的BGTZ目录，酷Q是\data\BGTZ\，mirai是\jre\bin\data\BGTZ\

