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


//agent1=chai.request.agent("https://localhost:44300/");
agent1 = chai.request.agent("https://costcodevops.azurewebsites.net");
describe('GetAllVertical', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Vertical/GetAllVertical')
		      //.get('dashboard/angular/project/views/ProjectUpdates.html')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response = res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				done();
		       });
    });

    it('Should return a json string', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });

    it('Json Array in the entry has Known Keys', function(done){
    	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					expect(result[i]).to.have.property('Key');
					expect(result[i]).to.have.property('Value');
					
				}
				return true;
			});
		done();
		
    })

    it('First entry in json object array has Known values', function(done){
		expect(result[0]).to.have.deep.property('Key',0);
	  	expect(result[0]).to.have.deep.property('Value','Warehouse_Solutions');	 
		done();
    });
     it('Second entry in json object array has Known values', function(done){
		expect(result[1]).to.have.deep.property('Key',1);
	  	expect(result[1]).to.have.deep.property('Value','Merchandising_Solutions');	 
		done();
    });
     it('Third entry in json object array has Known values', function(done){
		expect(result[2]).to.have.deep.property('Key',2);
	  	expect(result[2]).to.have.deep.property('Value','Membership_Solutions');	 
		done();
    });
     it('Fourth entry in json object array has Known values', function(done){
		expect(result[3]).to.have.deep.property('Key',3);
	  	expect(result[3]).to.have.deep.property('Value','Distribution_Solutions');	 
		done();
    });
     it('Fifth entry in json object array has Known values', function(done){
		expect(result[4]).to.have.deep.property('Key',4);
	  	expect(result[4]).to.have.deep.property('Value','International_Solutions');	 
		done();
    });
     it('Sixth entry in json object array has Known values', function(done){
		expect(result[5]).to.have.deep.property('Key',5);
	  	expect(result[5]).to.have.deep.property('Value','Ancillary_Solutions');	 
		done();
    });
     it('Seventh entry in json object array has Known values', function(done){
		expect(result[6]).to.have.deep.property('Key',6);
	  	expect(result[6]).to.have.deep.property('Value','eBusiness_Solutions');	 
		done();
    });
     it('Eighty entry in json object array has Known values', function(done){
		expect(result[7]).to.have.deep.property('Key',7);
	  	expect(result[7]).to.have.deep.property('Value','Corporate_Solutions');	 
		done();
    });
     it('Last entry in json object array has Known values', function(done){
		expect(result[8]).to.have.deep.property('Key',-1);
	  	expect(result[8]).to.have.deep.property('Value','Not_Assigned');	 
		done();
    }); 

describe('IsLogin', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Account/IsLogin')	
		      .set('Cookie','ai_user=X8pZz|2016-04-10T03:09:45.992Z; ARRAffinity=5d61d7e367d553a3c54df0aaec20b2ad4cadb57598e9e108062da519ca725375; ASP.NET_SessionId=ez0rkd1lid202f11dbxyxmw2; .AspNet.ExternalCookie=dj4wm8qck6OQAVpt0zoW1xj-0q2hALC8G7weV_t2Hymdgv8jlbt2MzJfSEEN50yCoAiu09W-5ITyKPVS7PQLcs4hSs_ANeEgoEOKsg3I8wukIJZkqBNi5EJJphlT15leAOpORtARUSK-ykk6EV2QB-VbMxgdw3kRRBU8jlYkAIkdoAelVY13VNuNCwhvoTB_i3Y4lDCE9VSbqu4oFISneRJOateQ6bscAfedD48PRIh6o7afwPE6QNZxcbWsXDpEVQ9vujqxo_gu43CMZp_nJgpr08Zmg95WtZUcVcyFG7uhxCoLKF_YoeCMEJgoy3TQ4o8jGzeL5qslEz2T-bppVDh1B2TXGgvJHrQupfGrQapaz2qfnqOzepq3DhkPc5Ojz6TL_wZ-Mgz1COS-DIIFwP_LItwJ8-jOa3sBi8tP1JdA6GHiNDzzZZODtmLToOBd')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				//console.log(result);
				done();
		       });
    });

    it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });

    it('Should return username "costcosu@gmail.com" for current session', function(done){
		expect(result).to.be.eql('costcosu@gmail.com');
		done();
	});

});

});

describe('GetVerticalProjects', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Vertical/GetVerticalProjects/0')
		      .set('Cookie','ai_user=X8pZz|2016-04-10T03:09:45.992Z; ARRAffinity=5d61d7e367d553a3c54df0aaec20b2ad4cadb57598e9e108062da519ca725375; ASP.NET_SessionId=ez0rkd1lid202f11dbxyxmw2; .AspNet.ExternalCookie=dj4wm8qck6OQAVpt0zoW1xj-0q2hALC8G7weV_t2Hymdgv8jlbt2MzJfSEEN50yCoAiu09W-5ITyKPVS7PQLcs4hSs_ANeEgoEOKsg3I8wukIJZkqBNi5EJJphlT15leAOpORtARUSK-ykk6EV2QB-VbMxgdw3kRRBU8jlYkAIkdoAelVY13VNuNCwhvoTB_i3Y4lDCE9VSbqu4oFISneRJOateQ6bscAfedD48PRIh6o7afwPE6QNZxcbWsXDpEVQ9vujqxo_gu43CMZp_nJgpr08Zmg95WtZUcVcyFG7uhxCoLKF_YoeCMEJgoy3TQ4o8jGzeL5qslEz2T-bppVDh1B2TXGgvJHrQupfGrQapaz2qfnqOzepq3DhkPc5Ojz6TL_wZ-Mgz1COS-DIIFwP_LItwJ8-jOa3sBi8tP1JdA6GHiNDzzZZODtmLToOBd')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				//console.log(res.text);
				done();
		       });
    });
    it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });
     it('Json Array in the entry has Known Keys', function(done){
    	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					expect(result[i]).to.have.property('LatestUpdate');
					expect(result[i]).to.have.property('ProjectID');
					expect(result[i]).to.have.property('ProjectName');
					expect(result[i]).to.have.property('Description');
					expect(result[i]).to.have.property('VerticalID');
					expect(result[i]).to.have.property('Vertical');
					expect(result[i]).to.have.property('ProjectPhases');
					expect(result[i]).to.have.property('ProjectUpdates');
					expect(result[i]).to.have.property('StatusUpdates');			
							
				}
				return true;
			});
		done();		
    });
	it('GetVerticalProjects returns ProjectID:509f534a-2c78-4cd5-bbf9-2349e9bb1420', function(done){
		expect(result[0]).to.have.deep.property('ProjectID','509f534a-2c78-4cd5-bbf9-2349e9bb1420');
	  	done();
    });
    it('GetVerticalProjects returns ProjectID:21ee5812-9d98-4a5a-a173-5f6ba4b7f267 ', function(done){
		expect(result[1]).to.have.deep.property('ProjectID','21ee5812-9d98-4a5a-a173-5f6ba4b7f267');
	  	done();
    });
     it('GetVerticalProjects returns ProjectID:2267cdcc-2fd0-4033-9eb0-82d31ae42516', function(done){
		expect(result[2]).to.have.deep.property('ProjectID','2267cdcc-2fd0-4033-9eb0-82d31ae42516');
	  	done();
    });
     it('GetVerticalProjects returns ProjectID:0a9514be-0e61-4ffc-9d7b-8e151a126038', function(done){
		expect(result[3]).to.have.deep.property('ProjectID','0a9514be-0e61-4ffc-9d7b-8e151a126038');
	  	done();
    });
      it('GetVerticalProjects returns ProjectID:6eec9e76-6ab0-414b-b03f-98597530853a', function(done){
		expect(result[4]).to.have.deep.property('ProjectID','6eec9e76-6ab0-414b-b03f-98597530853a');
	  	done();
    });
       it('GetVerticalProjects returns ProjectID: d09b5ddf-a92e-481b-9942-a404c2c2bdfd', function(done){
		expect(result[5]).to.have.deep.property('ProjectID','d09b5ddf-a92e-481b-9942-a404c2c2bdfd');
	  	done();
    });

});


describe('GetProjectUpdates', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/ProjectList/GetprojectUpdates/509f534a-2c78-4cd5-bbf9-2349e9bb1420')
		      .set('Cookie','ai_user=X8pZz|2016-04-10T03:09:45.992Z; ARRAffinity=5d61d7e367d553a3c54df0aaec20b2ad4cadb57598e9e108062da519ca725375; ASP.NET_SessionId=ez0rkd1lid202f11dbxyxmw2; .AspNet.ExternalCookie=dj4wm8qck6OQAVpt0zoW1xj-0q2hALC8G7weV_t2Hymdgv8jlbt2MzJfSEEN50yCoAiu09W-5ITyKPVS7PQLcs4hSs_ANeEgoEOKsg3I8wukIJZkqBNi5EJJphlT15leAOpORtARUSK-ykk6EV2QB-VbMxgdw3kRRBU8jlYkAIkdoAelVY13VNuNCwhvoTB_i3Y4lDCE9VSbqu4oFISneRJOateQ6bscAfedD48PRIh6o7afwPE6QNZxcbWsXDpEVQ9vujqxo_gu43CMZp_nJgpr08Zmg95WtZUcVcyFG7uhxCoLKF_YoeCMEJgoy3TQ4o8jGzeL5qslEz2T-bppVDh1B2TXGgvJHrQupfGrQapaz2qfnqOzepq3DhkPc5Ojz6TL_wZ-Mgz1COS-DIIFwP_LItwJ8-jOa3sBi8tP1JdA6GHiNDzzZZODtmLToOBd')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				//console.log(result);
				done();
		       });
    });

     it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.below(2);
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });

     it('Should return known Keys',function(done){
     	expect(result[0]).to.include.keys('$id');
		expect(result[0]).to.include.keys('Date');
		expect(result[0]).to.include.keys('Environment');
		expect(result[0]).to.include.keys('Description');
		expect(result[0]).to.include.keys('PhaseID');
		expect(result[0]).to.include.keys('ProjectUpdateID');
		expect(result[0]).to.include.keys('ProjectID');
		expect(result[0]).to.include.keys('Subject');
		expect(result[0]).to.include.keys('Project');
		expect(result[0]).to.include.keys('StatusUpdates');
		done();
     })

      it('GetprojectUpdates should return known values', function(done){
		expect(result[0]).to.have.deep.property('ProjectID','509f534a-2c78-4cd5-bbf9-2349e9bb1420');
		expect(result[0]).to.have.deep.property('ProjectUpdateID','135c96dd-2987-4118-94cf-f039baff94da');
		expect(result[0]).to.have.deep.property('Subject','Deployment Succeeded wdc_prod_group1: Ecomm_NonCore [RxWebInt_3.0.4877.0]');
		expect(result[0]).to.have.deep.property('Phase','Start_Up');
		
		done();
    });


});

describe('GetStatusData', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/ProjectList/GetStatusData/e22c5304-163c-4ba4-bd7e-20b71e41572f/1213a14b-4a19-4d84-93aa-1e18ea01f248')
		      .set('Cookie','ai_user=X8pZz|2016-04-10T03:09:45.992Z; ARRAffinity=5d61d7e367d553a3c54df0aaec20b2ad4cadb57598e9e108062da519ca725375; ASP.NET_SessionId=ez0rkd1lid202f11dbxyxmw2; .AspNet.ExternalCookie=dj4wm8qck6OQAVpt0zoW1xj-0q2hALC8G7weV_t2Hymdgv8jlbt2MzJfSEEN50yCoAiu09W-5ITyKPVS7PQLcs4hSs_ANeEgoEOKsg3I8wukIJZkqBNi5EJJphlT15leAOpORtARUSK-ykk6EV2QB-VbMxgdw3kRRBU8jlYkAIkdoAelVY13VNuNCwhvoTB_i3Y4lDCE9VSbqu4oFISneRJOateQ6bscAfedD48PRIh6o7afwPE6QNZxcbWsXDpEVQ9vujqxo_gu43CMZp_nJgpr08Zmg95WtZUcVcyFG7uhxCoLKF_YoeCMEJgoy3TQ4o8jGzeL5qslEz2T-bppVDh1B2TXGgvJHrQupfGrQapaz2qfnqOzepq3DhkPc5Ojz6TL_wZ-Mgz1COS-DIIFwP_LItwJ8-jOa3sBi8tP1JdA6GHiNDzzZZODtmLToOBd')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				//console.log(result);
				done();
		       });
    });

     it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });

     it('Json Array in the entry has Known Keys', function(done){
    	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					expect(result[i]).to.have.property('PhaseID');
					expect(result[i]).to.have.property('ProjectID');
					expect(result[i]).to.have.property('ProjectName');
					expect(result[i]).to.have.property('ProjectUpdateID');
					expect(result[i]).to.have.property('VerticalID');
					expect(result[i]).to.have.property('RecordDate');
					expect(result[i]).to.have.property('UpdateKey');
					expect(result[i]).to.have.property('UpdateValue');
					expect(result[i]).to.have.property('Phase');
					expect(result[i]).to.have.property('Project');
					expect(result[i]).to.have.property('ProjectUpdate');
					expect(result[i]).to.have.property('Vertical');	
					
				}
				return true;
			});
		done();		
    });


});

var emailJson = {
"ProjectName":"MochaTest6:Ecomm_NonCore",

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
				response=res;
				result = res.text;
			   // console.log(res.text);
				done();
		       });
    });

     it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
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
				response=res;
				result=res.text
				//console.log(res.text);
				done();
		       });
    });

     it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });
});

describe('Logoff', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .post('/Account/Logoff')
		      .set('Cookie','ai_user=X8pZz|2016-04-10T03:09:45.992Z; ARRAffinity=5d61d7e367d553a3c54df0aaec20b2ad4cadb57598e9e108062da519ca725375; ASP.NET_SessionId=yliqzw54b0l2sije2d0n142i; .AspNet.ExternalCookie=ADHisJ8nHEUzRuWmZ7oxzXlmE1fDYAEu8uxq1km1G8DEWgWMq0dig8JB5pImByH1owatOUudKRNDYK7MJxAhMYf5rjdhBqakQTN72WsMbWKan5SIBlOnYK4_FzEr1druKeJ2t_3phf2amOEAe8Fm87E8dh4p_C4K3IgtS-m8bClaVNUB58YUhtTMgteGJxgcqgyo1zzWT4Xw07ZRLnnJshjOtnxw9z29V_5W6rXBtL8Ut5l38wC6EaDWrPCl2XSxdXDQpMzA1IItj0Oh5r6Se48bUMTo5P21mzDTIrLpmVhZ4kbozzuNDPbYqA7_iZD0lHwmhf-1Aljug-2vxKDGV1tMUErv4fhuxEzTHm4gVqXY3w813O8oSZlBHomgi_zcW3sMem8DhGBxo4LY4M0CmdglDodf8OnpdlwmXl78lieyCek-UBUc8DbgzriZWzoY')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result=res.text;
				//console.log(res.text);
				done();
		       });
    });

     it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html');
		done();
    });
});

});


