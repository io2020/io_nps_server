rem ��ȡϵͳ����ԱȨ��
%1 mshta vbscript:CreateObject("Shell.Application").ShellExecute("cmd.exe","/c %~s0 ::","","runas",1)(window.close)&&exit

rem ���ݿ��Ƿ�ͬ����ṹ
setx NPS_DB_SYNCSTRUCTURE false /M

rem ���ݿ��Ƿ�ͬ������
setx NPS_DB_SYNCDATA false /M

rem ���ݿ�����
setx NPS_DB_DATETYPE 0 /M

rem ���ݿ����������ַ���
setx NPS_DB_MASTERCONNECTSTRING "Data Source=rm-2zel8sk911c1l1g6a8o.mysql.rds.aliyuncs.com;Port=3306;User ID=ionps;Password=ioNPS2020; Initial Catalog=ionps;Charset=utf8mb4; SslMode=none;Min pool size=1" /M

rem ���ݿ�ӿ������ַ���
setx NPS_DB_SLAVECONNECTSTRING "cpftu7fes6yg2ar7rn2j-rw4rm.rwlb.rds.aliyuncs.com" /M

rem �Ƿ�����Redis
setx NPS_DB_ISUSEDREDIS true /M

rem Redis�����ַ���
setx NPS_DB_REDISCONNECTSTRING "r-m5ep2cn21tb7lvk0hxpd.redis.rds.aliyuncs.com:6379,password=soonSmart_Redis,defaultDatabase=11" /M

setx NPS_AUTH_JWT_SECURITYKEY "nps-dotnetfive-SecurityKey" /M

setx NPS_AUTH_JWT_ISSUER "nps-dotnetfive-Issuer" /M

setx NPS_AUTH_JWT_AUDIENCE "Nps.Api" /M

setx NPS_AUTH_JWT_CRYPTOGRAPHY "nps-dotnetfive-cryptography" /M

rem ѩ��ID��������������ID��ȡֵ0-31
setx NPS_IDGENERATOR_DATACENTERID 1 /M

rem ѩ��ID��������������ID��ȡֵ0-31
setx NPS_IDGENERATOR_WORKEID 1 /M

rem NPSԶ�̷�������ַ
setx NPS_REMOTEHOST "http://8.131.77.125:7501/" /M

pause