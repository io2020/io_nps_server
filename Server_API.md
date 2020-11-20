Server API

<br/> 
<br/> 

---
开通隧道
```
POST /client/port/open
```
| 参数   | 含义                    |
| ------ | -----------------------|
| deviceRid | 客户端唯一标识 |
| List\<Integer> ports | 申请开通的端口号列表|


<br/> 


| 返回值   | 含义         |
| ------ | ----------------------- |
| serverInfo | nps服务器信息 |
| vKey | 客户端唯一密钥 |
| clientInfo | 客户端信息 |
| List\<channelInfo> | 隧道信息列表 |


<br/> 
<br/> 
<br/> 

---
查询隧道列表
```
POST /client/port/list
```
| 参数   | 含义                    |
| ------ | -----------------------|
| deviceRid | 客户端唯一标识 |
| keyword | 申请开通的端口号列表|

<br/> 


| 返回值   | 含义         |
| ------ | ----------------------- |
| serverInfo | nps服务器信息 |
| vKey | 客户端唯一密钥 |
| List\<channelInfo> | 隧道信息列表 |

<br/> 
<br/> 
<br/> 

---
查询单个隧道
```
POST /client/port/single
```
| 参数   | 含义                    |
| ------ | -----------------------|
| deviceRid | 客户端唯一标识 |
| channelId | 隧道id|

<br/> 


| 返回值   | 含义         |
| ------ | ----------------------- |
| serverInfo | nps服务器信息 |
| vKey | 客户端唯一密钥 |
| channelInfo | 隧道信息 |

<br/> 
<br/> 
<br/> 

---
编辑隧道
```
POST /client/port/edit
```
| 参数   | 含义                    |
| ------ | -----------------------|
| deviceRid | 客户端唯一标识 |
| channelId | 隧道id|

<br/> 


| 返回值   | 含义         |
| ------ | ----------------------- |
| serverInfo | nps服务器信息 |
| vKey | 客户端唯一密钥 |
| channelInfo | 隧道信息 |

<br/> 
<br/> 
<br/> 

---
删除隧道
```
POST /client/port/delete
```
| 参数   | 含义                    |
| ------ | -----------------------|
| deviceRid | 客户端唯一标识 |
| channelId | 隧道id|

<br/> 


| 返回值   | 含义         |
| ------ | ----------------------- |
| execResult | 操作结果 |

<br/> 
<br/> 
<br/> 



---
启动隧道
```
POST /client/port/start
```
| 参数   | 含义                    |
| ------ | -----------------------|
| deviceRid | 客户端唯一标识 |
| vKey | 协议端口对应密钥 |

<br/> 


| 返回值   | 含义         |
| ------ | ----------------------- |
| execResult | 操作结果 |


<br/> 
<br/> 
<br/> 

---
停止隧道
```
POST /client/port/stop
```
| 参数   | 含义                    |
| ------ | -----------------------|
| deviceRid | 客户端唯一标识 |
| vKey | 协议端口对应密钥 |

<br/> 

| 返回值   | 含义         |
| ------ | ----------------------- |
| execResult | 操作结果 |
