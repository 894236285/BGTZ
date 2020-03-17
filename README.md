## 简介

本应用基于[Native.SDK](https://github.com/Jie2GG/Native.Framework)，主要功能是获取起点中文网的书籍更新情况，如是否更新，更新的最新章节名称，字数等。

## 特点

* 同时获取多本书的更新情况
* 同时发送到多个QQ群
* 要获取的书籍和发送消息的QQ群可配置
* 即时发送QQ消息

## 实现思路
* 章节报更：在应用启动时，使用System.Timer的计时器，每隔一定时间从起点服务器获取书籍的章节列表，然后筛选出最新的章节，并发布到群内。(可配置书籍名称和要发布的群)
* 入群欢迎：在新成员新增事件中，发送群消息并@新群员。(可配置启用入群欢迎的群)

## 配置文件

1. 章节报更涉及2个文件配置，分别为Book.xml,xxx.json
* Book.xml示例格式及内容，其中id是起点中文网的书籍id,code和name是自定义命名，code会根据名称生成最新章节的json，name一般用书籍名称易于理解阅读，group节点是获取的章节信息将要发到哪些群
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
2. 入群欢迎涉及1个文件配置，即Group.xml,存储的是哪些群启用了入群欢迎
```xml 
<?xml version="1.0" encoding="utf-8" ?>
<root>
    <groups>
		<group>615387042</group>
  </groups>
</root>
```

## 说明
1. 代码中关于配置文件的加载获取都是写死了的，要修改的话只能直接修改代码，其中book.xml和group.xml的路径是F:\BookGroup\Book.xml和F:\BookGroup\Group.xml,json文件地址是F:\LatestChapter\xxx.json
2. core项目的生成目录是酷Q的dev目录，需修改。
