console.log('starting test');
var chai = require('chai');
var chaiHttp = require('chai-http');
var async = require('async');

var assert = chai.assert;
var expect = chai.expect;
var should = chai.should();
chai.use(chaiHttp);

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

//defines a test suite
describe('Business Unit Test Result', function () {

	this.timeout(15000);
	var results;
	var response;


agent1=chai.request.agent("https://localhost:44300/");

describe('IsLogin', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Account/IsLogin')	
		      .set('Cookie','ai_user=9z4tw|2016-05-13T23:20:16.657Z; ASP.NET_SessionId=ir2zqt1vqxhgaffxdsz54sn3; .AspNet.ExternalCookie=g533NMTMjeJhceykajfkJyCFNhr9_-BZcVxF2sKI9W-11Owqu5LCI_rukCyLVSG0L53XH9Tu6ttwo3Crj59SELJQyVbCqur7rXXihwAEeltRy2z0d9CqueaNAb3N3wU8CJG636Zj63U_r2oWuNhYNgFM1dmN_KPQr-xbMlG5WFGUl1Yr1ISYGx5fZX1CfuvKBQD9N8VczDxpzjTbJmnew2vOGzqKOBlz1x8KRGYIdZ_zurwO1x0lQ_WkxYVA-0Bw42MHjce8NIQAKpV1B4dJ7HvFlOQtNlkI3zvYCQIRbZpYG-Qtz52cZZMYZJwZeYcBYMKlBc9FeCq4LiLKZxWTuFtP2SxT4rdYmZqhmn28pmHnG4E3Z9DlhTb4c1EKzpArxzLOhwFNrzmfbnrGn6kcxaoItfn2pmBQd71LbxTZClZlzhx7d6CxshWa8de_QIRN')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				console.log(res.text);
				done();
		       });
    });

});


describe('GetAllVertical', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Vertical/GetAllVertical')
		      //.get('dashboard/angular/project/views/ProjectUpdates.html')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				console.log(res.text);
				done();
		       });
    });
});

describe('GetVerticalProjects', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Vertical/GetVerticalProjects/0')
		      .set('Cookie','ai_user=9z4tw|2016-05-13T23:20:16.657Z; ASP.NET_SessionId=ir2zqt1vqxhgaffxdsz54sn3; .AspNet.ExternalCookie=g533NMTMjeJhceykajfkJyCFNhr9_-BZcVxF2sKI9W-11Owqu5LCI_rukCyLVSG0L53XH9Tu6ttwo3Crj59SELJQyVbCqur7rXXihwAEeltRy2z0d9CqueaNAb3N3wU8CJG636Zj63U_r2oWuNhYNgFM1dmN_KPQr-xbMlG5WFGUl1Yr1ISYGx5fZX1CfuvKBQD9N8VczDxpzjTbJmnew2vOGzqKOBlz1x8KRGYIdZ_zurwO1x0lQ_WkxYVA-0Bw42MHjce8NIQAKpV1B4dJ7HvFlOQtNlkI3zvYCQIRbZpYG-Qtz52cZZMYZJwZeYcBYMKlBc9FeCq4LiLKZxWTuFtP2SxT4rdYmZqhmn28pmHnG4E3Z9DlhTb4c1EKzpArxzLOhwFNrzmfbnrGn6kcxaoItfn2pmBQd71LbxTZClZlzhx7d6CxshWa8de_QIRN')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				console.log(res.text);
				done();
		       });
    });
});

describe('GetProjectUpdates', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/ProjectList/GetprojectUpdates/e22c5304-163c-4ba4-bd7e-20b71e41572f')
		      .set('Cookie','ai_user=9z4tw|2016-05-13T23:20:16.657Z; ASP.NET_SessionId=ir2zqt1vqxhgaffxdsz54sn3; .AspNet.ExternalCookie=g533NMTMjeJhceykajfkJyCFNhr9_-BZcVxF2sKI9W-11Owqu5LCI_rukCyLVSG0L53XH9Tu6ttwo3Crj59SELJQyVbCqur7rXXihwAEeltRy2z0d9CqueaNAb3N3wU8CJG636Zj63U_r2oWuNhYNgFM1dmN_KPQr-xbMlG5WFGUl1Yr1ISYGx5fZX1CfuvKBQD9N8VczDxpzjTbJmnew2vOGzqKOBlz1x8KRGYIdZ_zurwO1x0lQ_WkxYVA-0Bw42MHjce8NIQAKpV1B4dJ7HvFlOQtNlkI3zvYCQIRbZpYG-Qtz52cZZMYZJwZeYcBYMKlBc9FeCq4LiLKZxWTuFtP2SxT4rdYmZqhmn28pmHnG4E3Z9DlhTb4c1EKzpArxzLOhwFNrzmfbnrGn6kcxaoItfn2pmBQd71LbxTZClZlzhx7d6CxshWa8de_QIRN')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				console.log(res.text);
				done();
		       });
    });
});

describe('GetStatusData', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/ProjectList/GetStatusData/e22c5304-163c-4ba4-bd7e-20b71e41572f/1213a14b-4a19-4d84-93aa-1e18ea01f248')
		      .set('Cookie','ai_user=9z4tw|2016-05-13T23:20:16.657Z; ASP.NET_SessionId=ir2zqt1vqxhgaffxdsz54sn3; .AspNet.ExternalCookie=RGrcFHnXQ2DkdTqvDbQl4FA2XbC5qVV6JtOIoi4NABCYPEPtpvTWJe-Gz4pWw4Kx6cebUzJ0jHxhqkRTr7z_wVjauWH7KBuv8ta6zPqOl35WdRt4R7gU-R95ooGQh8PSxmYZZqdsQj6gsXcfVp4_nKTjoTPr5x7TW-qHbx563vbWiFROdMxSWtI_ui-EPXlXm8Mi5ESs9E7UoiJPTPSMQKs15RKE39fJmxOWj_0Igg8hkDdZd_zshmsy2pihFZYIpiclxVqUEh1C4D3O5-_QlWj2yhMaG2UJccBiYQXUjEkSVenhIbA7oFmvSEBvU63sADf3gZ9TI1OjU83IdtPOkfNbq_zbgr_sVXRXUObWpE0OJ5732ZrH-WAC8Y2WdfaryukbF0xhfOWE1oWaA9gxNX58aSGJZR7rdkEH6VEaPQSSXD0u12yozABV3f0F5HuT')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				console.log(res.text);
				done();
		       });
    });
});


var emailJson = {
"ProjectName":"MochaTest2:Ecomm_NonCore",

"Subject":"Deployment Succeeded wdc_prod_group1: Ecomm_NonCore [RxWebInt_3.0.4877.0]",

"Body":"Execution of a process on Ecomm_NonCore has completed successfully:\r\n\r\nApplication:\r\n\r\nEcomm_NonCore\r\n\r\nProcess:\r\n\r\nDeploy\r\n\r\nSnapshot:\r\n\r\nRxWeb1603A\r\n\r\nEnvironment:\r\n\r\nwdc_prod_group1\r\n\r\nRequested By:\r\n\r\nmcarmod\r\n\r\nRequested On:\r\n\r\nMon, 18 Apr 2016 15:27:03 -0700\r\n\r\nDescription:\r\nExecution Summary:\r\n\r\n*Process*\r\n\r\n*Resource*\r\n\r\n*Start*\r\n\r\n*Duration*\r\n\r\n*Status*\r\nVersions Included:\r\n\r\n*Component*\r\n\r\n*Version*\r\n\r\n*Description*\r\n\r\nEcomm_RxWeb_InstallSQL\r\n\r\nRxWebInt_3.0.4877.0\r\n\r\nJenkins build: 10 SVN revision: SVN_REVISION_1\r\nNo Inventory Changes Made\r\n\r\nAdditional information available on uDeploy <https://udeploy.costco.com/>:\r\nhttps://udeploy.costco.com#applicationProcessRequest/64c9916a-3ea8-4fba-bdae-e7ff3fcaa72f\r\n<https://udeploy.costco.com/#applicationProcessRequest/64c9916a-3ea8-4fba-bdae-e7ff3fcaa72f>\r\n\r\n",

"Updates":[{"Key":"Application:","Value":"Ecomm_NonCore"},{"Key":"Process:","Value":"Deploy"},{"Key":"Environment:","Value":"wdc_prod_group1"},{"Key":"Requested By:","Value":"mcarmod"},{"Key":"Requested On:","Value":"Mon, 18 Apr 2016 15:27:03 -0700"},{"Key":"Description:","Value":""}]

}

describe('ProjectUpdate', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .post('/ProjectUpdate/Update')
		      .send(emailJson)
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				console.log(res.text);
				done();
		       });
    });
});

describe('Logoff', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .post('/Account/Logoff')
		      .set('Cookie','ai_user=9z4tw|2016-05-13T23:20:16.657Z; ASP.NET_SessionId=ir2zqt1vqxhgaffxdsz54sn3; .AspNet.ExternalCookie=RGrcFHnXQ2DkdTqvDbQl4FA2XbC5qVV6JtOIoi4NABCYPEPtpvTWJe-Gz4pWw4Kx6cebUzJ0jHxhqkRTr7z_wVjauWH7KBuv8ta6zPqOl35WdRt4R7gU-R95ooGQh8PSxmYZZqdsQj6gsXcfVp4_nKTjoTPr5x7TW-qHbx563vbWiFROdMxSWtI_ui-EPXlXm8Mi5ESs9E7UoiJPTPSMQKs15RKE39fJmxOWj_0Igg8hkDdZd_zshmsy2pihFZYIpiclxVqUEh1C4D3O5-_QlWj2yhMaG2UJccBiYQXUjEkSVenhIbA7oFmvSEBvU63sADf3gZ9TI1OjU83IdtPOkfNbq_zbgr_sVXRXUObWpE0OJ5732ZrH-WAC8Y2WdfaryukbF0xhfOWE1oWaA9gxNX58aSGJZR7rdkEH6VEaPQSSXD0u12yozABV3f0F5HuT')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				console.log(res.text);
				done();
		       });
    });
});

describe('Display', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .post('/ProjectList/Display')		      
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				console.log(res.text);
				done();
		       });
    });
});



});


