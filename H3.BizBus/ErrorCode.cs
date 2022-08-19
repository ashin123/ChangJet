using System;

namespace H3
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 通用型失败，对于H3调用外部的系统的时候，可能不会细分具体的错误代码，那么就使用这个错误代码
        /// </summary>
        GeneralFailed = 1,

        #region 通用型错误代码

        /// <summary>
        /// 创建的Item已经存在
        /// </summary>
        ItemExists = 10,

        /// <summary>
        /// 项目不存在
        /// </summary>
        ItemNotExists = 11,

        /// <summary>
        /// 输入的参数为空
        /// </summary>
        ArgumentIsNull = 12,

        /// <summary>
        /// 保存出错
        /// </summary>
        SaveFailed = 13,

        /// <summary>
        /// 参数不合法
        /// </summary>
        ParameterInvalid = 14,

        /// <summary>
        /// 编码已经存在
        /// </summary>
        CodeExists = 15,

        /// <summary>
        /// 编码被改变
        /// </summary>
        CodeIsNotChangable = 16,

        /// <summary>
        /// 编码字段不合法
        /// </summary>
        CodeInvalid = 17,

        /// <summary>
        /// ID不合法
        /// </summary>
        IdInvalid = 18,

        /// <summary>
        /// 名称不合法
        /// </summary>
        NameInvalid = 19,

        /// <summary>
        /// 名称重复
        /// </summary>
        NameDuplicated = 20,

        /// <summary>
        /// 组织的编码存在重复
        /// </summary>
        CodeDuplicated = 21,

        /// <summary>
        /// 组织的编码为空
        /// </summary>
        CodeIsNull = 22,

        /// <summary>
        /// 状态不合法，禁止这类操作
        /// </summary>
        InvalidState = 23,

        /// <summary>
        /// 非任务发送者
        /// </summary>
        NotSender = 24,

        /// <summary>
        /// Json字符串不合法
        /// </summary>
        JsonInvalid = 25,

        #endregion

        #region 集群

        /// <summary>
        /// 引擎不存在
        /// </summary>
        LogicUnitNotExists = 100,

        /// <summary>
        /// 引擎正在运行中
        /// </summary>
        LogicUnitIsRunning = 101,

        /// <summary>
        /// 加载引擎的时候出现异常
        /// </summary>
        LoadLogicUnitException = 102,

        /// <summary>
        /// 用户的编码为空
        /// </summary>
        UserCodeIsNull = 103,

        /// <summary>
        /// 用户编码已经存在
        /// </summary>
        UserCodeExists = 104,

        /// <summary>
        /// 用户的手机号已经存在
        /// </summary>
        UserMobileExists = 105,

        /// <summary>
        /// 用户的邮件地址已经存在
        /// </summary>
        UserEmailExists = 106,

        /// <summary>
        /// 注册引擎失败
        /// </summary>
        AddLogicUnitFailed = 107,

        /// <summary>
        /// 登录名已经存在了
        /// </summary>
        LoginNameExists = 108,

        /// <summary>
        /// 账户类型不合法
        /// </summary>
        AccountTypeInvalid = 109,

        /// <summary>
        /// 验证码错误
        /// </summary>
        VerificationCodeInvalid = 110,

        /// <summary>
        /// 钉钉ID已存在
        /// </summary>
        UserDingTalkAccountExists = 113,

        /// <summary>
        /// 没有可供使用的数据库服务器
        /// </summary>
        NoAvailableDbServer = 114,

        /// <summary>
        /// 无法分配数据库资源
        /// </summary>
        NoAvailableDatabase = 115,

        /// <summary>
        /// 创建数据库失败
        /// </summary>
        CreateDatabaseFailed = 116,

        /// <summary>
        /// 创建RDS账户失败
        /// </summary>
        CreateRdsDbAccountFailed = 117,

        /// <summary>
        /// 对RDS账户授权失败
        /// </summary>
        GrantRdsDbAccountFailed = 118,

        /// <summary>
        /// 连接到RDS数据库失败
        /// </summary>
        ConnectToRdsDbFailed = 119,

        /// <summary>
        /// 没有可以分配的数据库
        /// </summary>
        NoAllocatableDb = 120,

        /// <summary>
        /// 删除数据库失败
        /// </summary>
        DropDbFailed = 121,

        /// <summary>
        /// 初始化引擎失败
        /// </summary>
        InitializeLogicUnitFailed = 130,

        /// <summary>
        /// 服务器不存在 
        /// </summary>
        VesselNotExists = 131,

        /// <summary>
        /// 服务器不处于运行状态
        /// </summary>
        VesselNotRunning = 132,

        /// <summary>
        /// 禁止该IP访问系统
        /// </summary>
        IpBlocked = 133,

        /// <summary>
        /// 本IP访问的次数过多，这些访问通常是诸如：注册申请之类的
        /// </summary>
        TooManyVisitWithinIp = 134,

        /// <summary>
        /// IP地址不在白名单内
        /// </summary>
        IpNotInWhitelist = 135,

        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordInvalid = 137,

        /// <summary>
        /// 连接失败
        /// </summary>
        ConnectionFailed = 138,

        /// <summary>
        /// 权限不足
        /// </summary>
        NotEnoughPermission = 139,

        /// <summary>
        /// 服务器的地址不合法
        /// </summary>
        ServerAddressInvalid = 140,

        /// <summary>
        /// 引擎内部发生错误
        /// </summary>
        ServiceInternalError = 141,

        /// <summary>
        /// 本服务器不是主服务器
        /// </summary>
        NotMaster = 142,

        /// <summary>
        /// 指定的逻辑单元不处于运行状态
        /// </summary>
        LogicUnitNotRunning = 143,

        /// <summary>
        /// 用户被禁用了
        /// </summary>
        UserDisabled = 144,

        /// <summary>
        /// 由于密码输入次数过多，用户被锁了
        /// </summary>
        UserLocked = 145,

        /// <summary>
        /// 集群令牌不合法
        /// </summary>
        ClusterTokenInvalid = 146,

        /// <summary>
        /// 申请次数太多
        /// </summary>
        ApplyTooMany = 147,

        /// <summary>
        /// 正在排队注册中
        /// </summary>
        RegisteringLogicUnit = 148,

        /// <summary>
        /// 钉钉企业已经存在了
        /// </summary>
        DingTalkCoprIdExists = 150,

        /// <summary>
        /// 安装应用失败
        /// </summary>
        InstallAppFailed = 151,

        /// <summary>
        /// 指定的逻辑单元未被加载
        /// </summary>
        LogicUnitNotMounted = 152,

        /// <summary>
        /// 钉钉ID不合法
        /// </summary>
        DingTalkIdInvalid = 153,

        DingTalkCorpIdNotExists = 154,

        DingTalkCorpExistsUserNotExists = 155,

        DingTalkSignatureNotExists = 156,

        DingTalkServiceNotRunning = 157,

        DingTalkPermanentCodeGetError = 158,

        /// <summary>
        /// 钉钉ID已存在
        /// </summary>
        UserDingIdAccountExists = 159,
        /// <summary>
        /// 钉钉扫码登录随机码失败
        /// </summary>
        DingTalkScanFailed = 160,
        /// <summary>
        /// 钉钉套件未找到
        /// </summary>
        DingTalkSuiteNotExists = 161,
        /// <summary>
        /// DingTalkCorp已删除
        /// </summary>
        DingTalkCorpNotExists = 162,
        /// <summary>
        /// 钉钉存在，引擎不存在
        /// </summary>
        DingTalkExistEngineNotExists = 163,
        /// <summary>
        /// 钉钉后台登陆H3验证码不存在
        /// </summary>
        DingTalkISVBackEndSignatureNotExists = 164,

        /// <summary>
        /// 获取钉钉AccessToken失败
        /// </summary>
        DingTalkAccessTokenFailed = 165,

        /// <summary>
        /// 编译失败
        /// </summary>
        CompileError = 170,

        /// <summary>
        /// 更新失败
        /// </summary>
        UpdateFailed = 171,

        /// <summary>
        /// 无法通过本方式登录生产环境
        /// </summary>
        CanNotLoginProductSystem = 172,

        #endregion

        #region 组织结构

        /// <summary>
        /// 管理员属性不合法
        /// </summary>
        ManagerInvalid = 201,

        /// <summary>
        /// 父属性不合法，为空或者不存在
        /// </summary>
        ParentIsNull = 203,

        /// <summary>
        /// 公司属性不合法，为空或者不存在
        /// </summary>
        CompanyInvalid = 204,

        /// <summary>
        /// 组织单元形成了环，不合法
        /// </summary>
        OrganizationUnitCycleInvalid = 205,

        /// <summary>
        /// 无法路由的根
        /// </summary>
        IterateToCompanyFailed = 206,

        /// <summary>
        /// 公司不一致
        /// </summary>
        CompanyInconsistent = 207,

        /// <summary>
        /// 公司不存在
        /// </summary>
        CompanyNotExists = 208,

        /// <summary>
        /// 父对象必须为OU或者公司
        /// </summary>
        ParentInvalid = 209,

        /// <summary>
        /// 员工编号存在重复
        /// </summary>
        EmployeeNumberDuplicated = 210,

        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExists = 212,

        /// <summary>
        /// 至少保留一个系统管理员
        /// </summary>
        AtLeastOneAdministrator = 213,

        /// <summary>
        /// 只有手机能够重置密码
        /// </summary>
        MobileRequired = 214,

        /// <summary>
        /// 用户的角色不存在
        /// </summary>
        OrgRoleNotExists = 215,

        /// <summary>
        /// 管理员角色不能设置编制的部门，因为管理员角色是不分部门的
        /// </summary>
        AdminRoleCanNotSetDepts = 216,

        /// <summary>
        /// 对于同一个用户，定义了两个相同的角色下的岗位
        /// </summary>
        DuplicatedRoles = 217,

        /// <summary>
        /// 每个用户默认都是所有用户角色，不需要单独再添加对应的岗位
        /// </summary>
        EveryoneIsUserRoleByDefault = 218,

        /// <summary>
        /// 岗位对应的部门不合法，为空或者不存在，或者重复
        /// </summary>
        PostDeptInvalid = 219,

        /// <summary>
        /// 用户只能从钉钉上同步过来，不能添加用户
        /// </summary>
        CanNotAddUser = 220,

        /// <summary>
        /// 不能变更用户的登录信息，只能从钉钉同步过来
        /// </summary>
        CanNotUpdateUserLoginName = 221,
        /// <summary>
        /// 管理员角色用户数不能为零
        /// </summary>
        AdminRoleUserNumberNotBeZero,

        #endregion

        #region 流程

        /// <summary>
        /// 流程实例不存在
        /// </summary>
        WorkflowInstanceNotExists = 300,

        /// <summary>
        /// 流程模板族没有找到
        /// </summary>
        WorkflowClauseNotExists = 301,

        /// <summary>
        /// 工作项不存在
        /// </summary>
        WorkItemNotExists = 302,

        /// <summary>
        /// 要做的任务处于Working状态
        /// </summary>
        WorkItemStateNotMatched = 303,

        /// <summary>
        /// 参与者重复
        /// </summary>
        WorkItemParticipantDuplicated = 304,

        #endregion

        #region 报表

        /// <summary>
        /// 报表配置为空
        /// </summary>
        ReportSettingNotExists = 400,

        /// <summary>
        /// 报表数据源不合法
        /// </summary>
        ReportSourceInvalid = 401,

        /// <summary>
        /// 报表配置错误
        /// </summary>
        ReportSettingInvalid = 402,

        #endregion

        #region 应用

        /// <summary>
        /// 表单不存在
        /// </summary>
        FormNotExists = 501,

        /// <summary>
        /// 业务对象模式编码重复
        /// </summary>
        SchemaCodeDuplicated = 503,

        /// <summary>
        /// 业务对象模式不存在
        /// </summary>
        SchemaNotExists = 504,

        /// <summary>
        /// 业务对象模式的行太长了
        /// </summary>
        SchemaRowTooLarge = 505,

        /// <summary>
        /// 业务对象模式的列冲突
        /// </summary>
        SchemaColumnConflicted = 506,

        /// <summary>
        /// 业务对象引用了正在被删除的对象
        /// </summary>
        BizObjectReferToRemovingObjects = 507,

        /// <summary>
        /// 子业务对象无法单独创建、删除和更新
        /// </summary>
        ChildBizObjectCanNotInvokeMethodAlone = 508,

        /// <summary>
        /// 不能删除一个被引用的对象
        /// </summary>
        CanNotRemoveObjectWhichReferred = 509,

        /// <summary>
        /// 不能删除有流程实例的对象
        /// </summary>
        CanNotRemoveObjectWithWorkflowInstance = 510,

        /// <summary>
        /// 不符合提交规则
        /// </summary>
        ValidateSubmitRuleFailed = 511,

        /// <summary>
        /// 创建业务规则对应的存储过程失败
        /// </summary>
        CreateBizRuleProcedureFailed = 512,

        /// <summary>
        /// 对象正在被删除
        /// </summary>
        ObjectIsRemoving = 513,

        /// <summary>
        /// 节点不存在
        /// </summary>
        NodeNotExists = 514,

        /// <summary>
        /// 业务对象正在被其他用户更新
        /// </summary>
        BizObjectIsUpdating = 515,

        /// <summary>
        /// 业务对象太大了
        /// </summary>
        BizObjectIsTooLarge = 516,

        #endregion

        #region BizBus

        /// <summary>
        /// 获取第三方系统的令牌失败，这主要是用于第三方系统的情况下，获得第三方系统中的令牌的情况下使用的
        /// </summary>
        LoadAccessPointTokenFailed = 600,
        /// <summary>
        /// 已经绑定引擎
        /// </summary>
        HaveAttached = 601,
        /// <summary>
        /// 未获得授权标识
        /// </summary>
        NotAuthroized = 603,
        /// <summary>
        /// 正在同步组织机构
        /// </summary>
        SynchronizingOrganization = 604,

        #endregion

        #region 集成

        /// <summary>
        /// 当前许可不支持调用Web Service
        /// </summary>
        LicenseNotSupportWebService = 700

        #endregion
    }
}
