# assignment

CLO VF Platform Backend 과제 저장소 입니다.
-
> **특이사항**<br/>
> - json 및 csv 파일 업로드시 &lt;input type="file" /&gt;로 업로드 요구 사항에서 file input 태그에 name 및 id 값이 **file** 로 설정 되어 있어야 합니다.
> - json 및 csv 양식을 &lt;textarea&gt;&lt;textarea/&gt;로 직접 입력시 textarea 태그에 name 및 id 값이 **textData** 로 설정 되어 있어야 합니다.
> - 데이터는 File DB Sqlite로 보관 되며, DB 위치는 프로젝트 루트 경로 밑에 **Database** 디렉토리에 생성 됩니다.<br/>이에 따라 다른 문제의 발생 여지를 줄이기 위해 프로젝트 실행시 VS를 관리자 권한으로 실행해서 확인해 주세요.

***

🛠️ 개발 환경 정보
-

- IDE : VS 2022
- Language : C# (ASP.NET Core .NET6)

📕 library to use
-

| Name | Version |
| --- | --- |
| **Microsoft.EntityFrameworkCore.Sqlite**<br/>File DB | 7.0.5
| **Microsoft.EntityFrameworkCore.Tools**<br/>DB 생성 및 마이그레이션 | 7.0.5 |
| **NLog.Web.AspNetCore**<br/>Log | 5.3.0
| **Swashbuckle.AspNetCore**<br />Open API Space 노출 | 6.2.3 |

✅ 제공 기능
-

- [x] 직원 연락망 정보 조회 [페이징]
- [x] 직원 연락망 검색 [By Name]
- [x] 직원 연락망 업로드 [json, csv 파일 업로드 및 Form 직접 입력]
- [x] 직원 연락망 삭제 [By Email]
- [x] 직원 연락망 수정 [By Id or Email]
