using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SIMSAssignment.API.Domain;
using SIMSAssignment.API.Models;
using SIMSAssignment.API.Services;

namespace SIMSAssignment.API
    {
    public class Startup
        {
        public Startup ( IConfiguration configuration )
            {
            Configuration = configuration;
            }

        public IConfiguration Configuration
            {
            get;
            }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices ( IServiceCollection services )
            {
            services.AddDbContext<SIMSDbContext>(opt => opt.UseInMemoryDatabase("SIMSDb"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                        {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        };
                });

            services.AddCors(cfg =>
            {
                cfg.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials());
            });


            services.AddScoped<UsersService>();
            services.AddScoped<EmployeesService>();
            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure ( IApplicationBuilder app, IHostingEnvironment env )
            {
            if (env.IsDevelopment())
                {
                app.UseDeveloperExceptionPage();
                }

            app.UseAuthentication();
            app.UseCors("CorsPolicy");

            AddTestData(app);

            app.UseHttpsRedirection();
            app.UseMvc();
            }



        // Seed the in-memory database
        private static void AddTestData ( IApplicationBuilder app )
            {
            using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                using (var context = serviceScope.ServiceProvider.GetService<SIMSDbContext>())
                    {
                    // Create the roles
                    context.Roles.Add(new SIMSRole { Id = "Manager" });
                    context.Roles.Add(new SIMSRole { Id = "SubManager" });
                    context.Roles.Add(new SIMSRole { Id = "Receptionist" });

                    // create the claims
                    context.Claims.Add(new SIMSClaim { Id = "ViewSSN" });
                    context.Claims.Add(new SIMSClaim { Id = "ViewDL" });

                    // Create the 3 users with their roles and claims
                    var ManagerUser = new SIMSUser()
                        {
                        UserID = "Manager",
                        Password = "Pass123!",
                        FirstName = "Luke",
                        LastName = "Skywalker",
                        Roles = "Manager",
                        Claims = "ViewSSN,ViewDL"
                        };

                    context.Users.Add(ManagerUser);

                    var SubManagerUser = new SIMSUser()
                        {
                        UserID = "SubManager",
                        Password = "Pass123!",
                        FirstName = "Peter",
                        LastName = "Parker",
                        Roles = "SubManager",
                        Claims = "ViewDL"
                        };
                    context.Users.Add(SubManagerUser);

                    var ReceptionistUser = new SIMSUser()
                        {
                        UserID = "Receptionist",
                        Password = "Pass123!",
                        FirstName = "Bruce",
                        LastName = "Banner",
                        Roles = "Receptionist"
                        };
                    context.Users.Add(ReceptionistUser);

                    List<Employee> Emps = new List<Employee>() { //https://www.generatedata.com/
    new Employee { FirstName = "Quin", LastName = "Manning", Department = "HR", Email = "sociis@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "C.", Phone = "1-799-728-1246",SSN = "310817745", DriverLicense = "617665-5998" },
    new Employee { FirstName = "Whoopi", LastName = "Gay", Department = "TechSupport", Email = "bibendum@Integer.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-692-827-9719",SSN = "220114601", DriverLicense = "545800-2176" },
    new Employee { FirstName = "Jenette", LastName = "Petersen", Department = "Accounting", Email = "hendrerit.a@cubilia.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "C.", Phone = "1-302-740-6193",SSN = "320428359", DriverLicense = "444445-1167" },
    new Employee { FirstName = "Kenyon", LastName = "Mcgowan", Department = "HR", Email = "in.consequat@lorem.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-193-112-6075",SSN = "581113617", DriverLicense = "798004-0088" },
    new Employee { FirstName = "Jolene", LastName = "Britt", Department = "HR", Email = "Donec.sollicitudin@adipiscingnon.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-649-820-4673",SSN = "270330367", DriverLicense = "093673-9713" },
    new Employee { FirstName = "Russell", LastName = "Cooley", Department = " IT", Email = "Phasellus@inhendreritconsectetuer.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-264-194-6105",SSN = "230622378", DriverLicense = "626510-5723" },
    new Employee { FirstName = "Teegan", LastName = "Allison", Department = "TechSupport", Email = "non@nulla.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-913-711-0348",SSN = "530130542", DriverLicense = "588444-0958" },
    new Employee { FirstName = "Lillian", LastName = "Rogers", Department = "HR", Email = "luctus.lobortis.Class@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-751-703-6250",SSN = "740229634", DriverLicense = "479682-1215" },
    new Employee { FirstName = "Riley", LastName = "Whitley", Department = "Accounting", Email = "euismod@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "C.", Phone = "1-110-144-9420",SSN = "740513691", DriverLicense = "219913-8062" },
    new Employee { FirstName = "Tamekah", LastName = "Snider", Department = " IT", Email = "habitant@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-665-759-3961",SSN = "110306839", DriverLicense = "732402-7544" },
    new Employee { FirstName = "Mollie", LastName = "Mullins", Department = "TechSupport", Email = "ut@Donec.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-431-481-7841",SSN = "901223424", DriverLicense = "863650-2737" },
    new Employee { FirstName = "Chadwick", LastName = "Mccarty", Department = "HR", Email = "faucibus@mod.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-897-767-4618",SSN = "170828643", DriverLicense = "184216-6934" },
    new Employee { FirstName = "Ruth", LastName = "Sandoval", Department = "TechSupport", Email = "dictum.ultricies.ligula@miac.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-903-354-3121",SSN = "140201874", DriverLicense = "662919-7382" },
    new Employee { FirstName = "Camille", LastName = "Hardin", Department = "HR", Email = "urna@viverraDonectempus.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-616-793-9880",SSN = "520819006", DriverLicense = "633095-1689" },
    new Employee { FirstName = "Madeson", LastName = "Burgess", Department = "Accounting", Email = "tincidunt@nisiAenean.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-474-531-5612",SSN = "050325284", DriverLicense = "646378-2620" },
    new Employee { FirstName = "Brian", LastName = "Camacho", Department = " IT", Email = "varius@lectusquis.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-107-560-8609",SSN = "260503322", DriverLicense = "219526-4987" },
    new Employee { FirstName = "Guy", LastName = "Bass", Department = "HR", Email = "pharetra@Donec.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-213-205-8578",SSN = "160122007", DriverLicense = "962582-8893" },
    new Employee { FirstName = "Chava", LastName = "Carney", Department = "HR", Email = "blandit.at.nisi@molestiedapibusligula.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-522-533-7252",SSN = "080922760", DriverLicense = "777706-6155" },
    new Employee { FirstName = "Althea", LastName = "Mason", Department = "Accounting", Email = "Nam@inconsectetuer.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-574-366-5060",SSN = "930414343", DriverLicense = "336156-7435" },
    new Employee { FirstName = "Xaviera", LastName = "Fuller", Department = "TechSupport", Email = "et.eros.gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-788-128-0009",SSN = "270908087", DriverLicense = "696456-2570" },
    new Employee { FirstName = "Scarlet", LastName = "Mosley", Department = "Accounting", Email = "idunt@egetmollis.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-792-324-8270",SSN = "140418983", DriverLicense = "565579-3403" },
    new Employee { FirstName = "Kaye", LastName = "English", Department = "TechSupport", Email = "faucibus.lectus@Nulla.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-557-486-6408",SSN = "460128770", DriverLicense = "332653-2763" },
    new Employee { FirstName = "Karyn", LastName = "Juarez", Department = "HR", Email = "Mauris@eudolor.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-664-280-2499",SSN = "150318798", DriverLicense = "243917-0305" },
    new Employee { FirstName = "Hakeem", LastName = "Atkinson", Department = " IT", Email = "vulputate@sit.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-991-535-8037",SSN = "860302369", DriverLicense = "546813-5909" },
    new Employee { FirstName = "Leo", LastName = "Lloyd", Department = "TechSupport", Email = "et@euismodurna.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-478-830-0433",SSN = "731007801", DriverLicense = "774293-6557" },
    new Employee { FirstName = "Mechelle", LastName = "Mcgee", Department = "Accounting", Email = "turpis@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-914-878-1323",SSN = "961108780", DriverLicense = "593021-2583" },
    new Employee { FirstName = "Derek", LastName = "Russell", Department = "HR", Email = "amet@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-726-431-4687",SSN = "810627469", DriverLicense = "798320-8948" },
    new Employee { FirstName = "Garrett", LastName = "Chase", Department = "HR", Email = "habitant@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "S.", Phone = "1-958-364-4215",SSN = "530327955", DriverLicense = "953939-1921" },
    new Employee { FirstName = "Fuller", LastName = "Mccall", Department = " IT", Email = "auctor.quis@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-569-186-6753",SSN = "331217499", DriverLicense = "222714-1021" },
    new Employee { FirstName = "Geraldine", LastName = "Rodgers", Department = "TechSupport", Email = "metus@eu.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-505-669-5731",SSN = "280806177", DriverLicense = "609985-1120" },
    new Employee { FirstName = "Craig", LastName = "Guerra", Department = "TechSupport", Email = "amet@tempus.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-576-404-2304",SSN = "410702662", DriverLicense = "195399-1617" },
    new Employee { FirstName = "Nina", LastName = "Drake", Department = "Accounting", Email = "adipiscing@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-102-165-4484",SSN = "660601355", DriverLicense = "484855-6827" },
    new Employee { FirstName = "Igor", LastName = "Ruiz", Department = "HR", Email = "sem@ipsum.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-832-751-9382",SSN = "750930317", DriverLicense = "973749-5995" },
    new Employee { FirstName = "Victor", LastName = "Gardner", Department = "HR", Email = "vel.pede@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-593-145-7158",SSN = "631101339", DriverLicense = "965879-5043" },
    new Employee { FirstName = "Sarah", LastName = "Gonzales", Department = "Accounting", Email = "ante@blandit.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-840-425-9530",SSN = "510802386", DriverLicense = "323941-4059" },
    new Employee { FirstName = "Gillian", LastName = "Pratt", Department = "TechSupport", Email = "non.justo@turpis.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "S.", Phone = "1-455-686-6965",SSN = "920724622", DriverLicense = "543115-0761" },
    new Employee { FirstName = "Abra", LastName = "Stone", Department = "Accounting", Email = "nec@malesuadavel.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-182-502-9355",SSN = "610217391", DriverLicense = "508129-1949" },
    new Employee { FirstName = "Lois", LastName = "Vinson", Department = "TechSupport", Email = "fames@vel.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-937-978-2924",SSN = "390724298", DriverLicense = "274295-2894" },
    new Employee { FirstName = "Cameron", LastName = "Gallagher", Department = "HR", Email = "Nam@pede.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-573-767-2003",SSN = "590324837", DriverLicense = "808486-7129" },
    new Employee { FirstName = "Ulla", LastName = "Glover", Department = "Accounting", Email = "lacus@terdum.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-258-186-9826",SSN = "460419728", DriverLicense = "007741-6642" },
    new Employee { FirstName = "Noelani", LastName = "Church", Department = " IT", Email = "urna@purusDuis.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-179-774-5124",SSN = "960229720", DriverLicense = "457851-2248" },
    new Employee { FirstName = "Nina", LastName = "Brown", Department = "HR", Email = "accumsan@nulla.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-232-333-2215",SSN = "141219718", DriverLicense = "966784-1523" },
    new Employee { FirstName = "Jack", LastName = "Gregory", Department = " IT", Email = "pede@esque.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-703-676-3761",SSN = "910222792", DriverLicense = "306870-7664" },
    new Employee { FirstName = "Raphael", LastName = "Keith", Department = "Accounting", Email = "Phasellus@senectus.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "C.", Phone = "1-150-688-3561",SSN = "471227956", DriverLicense = "039965-4631" },
    new Employee { FirstName = "Teagan", LastName = "Sharpe", Department = "Accounting", Email = "nec.quam@laciniaat.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-804-463-3470",SSN = "490127712", DriverLicense = "476235-2807" },
    new Employee { FirstName = "Amir", LastName = "Goff", Department = "TechSupport", Email = "et.libero@pharetra.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-106-690-4417",SSN = "240504636", DriverLicense = "908892-8016" },
    new Employee { FirstName = "Rae", LastName = "Wyatt", Department = "Accounting", Email = "nec@odio.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-267-427-3231",SSN = "560820175", DriverLicense = "346990-3359" },
    new Employee { FirstName = "Mannix", LastName = "Rodgers", Department = "Accounting", Email = "element@lamcorper.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-757-141-7069",SSN = "470910682", DriverLicense = "845328-3627" },
    new Employee { FirstName = "Lilah", LastName = "Roman", Department = "Accounting", Email = "consect@arcuSedet.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "C.", Phone = "1-932-281-2744",SSN = "380510816", DriverLicense = "733768-2327" },
    new Employee { FirstName = "Vivien", LastName = "Cherry", Department = "TechSupport", Email = "Curabitur@risus.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-541-662-5140",SSN = "600826788", DriverLicense = "172577-1305" },
    new Employee { FirstName = "May", LastName = "Black", Department = " IT", Email = "ornare@egestas.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-724-128-7018",SSN = "351002971", DriverLicense = "035502-1338" },
    new Employee { FirstName = "Rigel", LastName = "Ferguson", Department = "TechSupport", Email = "risus@tellusnon.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-436-316-2590",SSN = "290525388", DriverLicense = "922777-6888" },
    new Employee { FirstName = "Jamalia", LastName = "Solomon", Department = "TechSupport", Email = "pede@magnaPhasellus.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-543-284-7220",SSN = "181225346", DriverLicense = "755119-4017" },
    new Employee { FirstName = "Hamish", LastName = "Justice", Department = " IT", Email = "odio@musProinvel.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-101-884-1636",SSN = "950203180", DriverLicense = "892452-4906" },
    new Employee { FirstName = "Kevin", LastName = "Clayton", Department = "Accounting", Email = "auctor.@id.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-548-110-4414",SSN = "980616740", DriverLicense = "854270-8212" },
    new Employee { FirstName = "Carla", LastName = "Hart", Department = "TechSupport", Email = "est@etrisusQuisque.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-967-127-5321",SSN = "381012650", DriverLicense = "108257-7931" },
    new Employee { FirstName = "Clayton", LastName = "Leach", Department = "Accounting", Email = "Maecenas@Proinvelnisl.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "S.", Phone = "1-479-191-8739",SSN = "791020716", DriverLicense = "974368-9169" },
    new Employee { FirstName = "Roth", LastName = "Hamilton", Department = "Accounting", Email = "laoreet@Integer.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-867-135-5637",SSN = "981224842", DriverLicense = "638537-2690" },
    new Employee { FirstName = "Sylvia", LastName = "Barnett", Department = " IT", Email = "adipiscing@getipsum.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-563-370-9624",SSN = "270204002", DriverLicense = "427124-3299" },
    new Employee { FirstName = "Willow", LastName = "Donovan", Department = "TechSupport", Email = "volutpat@Mauris.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-532-993-0682",SSN = "990507472", DriverLicense = "594163-8602" },
    new Employee { FirstName = "Barclay", LastName = "White", Department = "Accounting", Email = "posuere@dui.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-751-583-3076",SSN = "250502606", DriverLicense = "848977-8913" },
    new Employee { FirstName = "Robin", LastName = "Scott", Department = "TechSupport", Email = "rhoncus@sed.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-853-838-3050",SSN = "030503597", DriverLicense = "204627-6750" },
    new Employee { FirstName = "Edan", LastName = "Velez", Department = "TechSupport", Email = "sociis@ornare.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "S.", Phone = "1-223-430-6082",SSN = "810619594", DriverLicense = "886705-8078" },
    new Employee { FirstName = "Kirestin", LastName = "Palmer", Department = "Accounting", Email = "mauris@fringilla.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-556-931-8257",SSN = "220917631", DriverLicense = "714560-0016" },
    new Employee { FirstName = "Phoebe", LastName = "Lindsey", Department = "Accounting", Email = "erat.Etiam@sumcur.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-108-500-4816",SSN = "150221251", DriverLicense = "985987-5164" },
    new Employee { FirstName = "Lyle", LastName = "Love", Department = " IT", Email = "tdunt@diam.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-914-359-2183",SSN = "160301701", DriverLicense = "115730-2678" },
    new Employee { FirstName = "Vivien", LastName = "Mullins", Department = "HR", Email = "rutrum@id.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-377-561-5638",SSN = "921018097", DriverLicense = "543013-2703" },
    new Employee { FirstName = "Wing", LastName = "George", Department = "HR", Email = "sem@metussit.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-413-935-0475",SSN = "330716958", DriverLicense = "687210-2618" },
    new Employee { FirstName = "Jordan", LastName = "Santana", Department = "HR", Email = "malesuada@Cras.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-922-389-4019",SSN = "560723605", DriverLicense = "789542-7834" },
    new Employee { FirstName = "Lenore", LastName = "Shannon", Department = " IT", Email = "lum@nonummy.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-567-161-5971",SSN = "440728896", DriverLicense = "655089-4452" },
    new Employee { FirstName = "Nicole", LastName = "Cole", Department = "TechSupport", Email = "vulput@Aliqua.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "S.", Phone = "1-112-533-2972",SSN = "840606315", DriverLicense = "879699-3395" },
    new Employee { FirstName = "Stone", LastName = "Underwood", Department = "TechSupport", Email = "Duis@litoratorquent.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-866-732-8409",SSN = "240902579", DriverLicense = "982178-8271" },
    new Employee { FirstName = "Kylan", LastName = "Byers", Department = "Accounting", Email = "ridiculus@feugiat.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-470-987-1757",SSN = "520224252", DriverLicense = "778045-0065" },
    new Employee { FirstName = "Samson", LastName = "Suarez", Department = "TechSupport", Email = "bandit@bulumante.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-634-410-0404",SSN = "960425557", DriverLicense = "354193-1022" },
    new Employee { FirstName = "Pearl", LastName = "Fry", Department = " IT", Email = "nascetur@Integer.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-240-855-5889",SSN = "641209737", DriverLicense = "716843-7387" },
    new Employee { FirstName = "Tucker", LastName = "Wallace", Department = " IT", Email = "velit@luctus.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "C.", Phone = "1-881-915-2300",SSN = "830606242", DriverLicense = "665316-9349" },
    new Employee { FirstName = "Wynter", LastName = "Wells", Department = " IT", Email = "dolor@ante.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-592-580-7408",SSN = "900224473", DriverLicense = "919196-2209" },
    new Employee { FirstName = "Katell", LastName = "Cantrell", Department = "TechSupport", Email = "Duis@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "S.", Phone = "1-987-862-9596",SSN = "321004730", DriverLicense = "449098-0804" },
    new Employee { FirstName = "Halla", LastName = "Barker", Department = "Accounting", Email = "Xeres@convallis.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-541-201-0177",SSN = "150706116", DriverLicense = "323458-5291" },
    new Employee { FirstName = "Murphy", LastName = "Simmons", Department = "TechSupport", Email = "sit@nonarcu.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-608-211-0859",SSN = "440813724", DriverLicense = "577331-8323" },
    new Employee { FirstName = "Chava", LastName = "Chan", Department = " IT", Email = "blandit@accumsan.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "S.", Phone = "1-981-308-7827",SSN = "910405733", DriverLicense = "086391-0113" },
    new Employee { FirstName = "Cathleen", LastName = "Eaton", Department = "Accounting", Email = "sem.magna@commodo.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-350-603-0683",SSN = "321010543", DriverLicense = "483696-8422" },
    new Employee { FirstName = "Audra", LastName = "Glover", Department = "HR", Email = "elit@amet.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "C.", Phone = "1-357-677-5930",SSN = "460221737", DriverLicense = "259524-8366" },
    new Employee { FirstName = "Martha", LastName = "Meyers", Department = "Accounting", Email = "sit@nibhsitamet.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-402-104-2992",SSN = "351127943", DriverLicense = "315307-0853" },
    new Employee { FirstName = "Roth", LastName = "Rowe", Department = "TechSupport", Email = "euismod@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-960-945-4233",SSN = "050201121", DriverLicense = "517662-0051" },
    new Employee { FirstName = "Nehru", LastName = "Vincent", Department = "Accounting", Email = "mattis@arcuSed.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "F.", Phone = "1-248-766-6856",SSN = "070306409", DriverLicense = "574689-5704" },
    new Employee { FirstName = "Berk", LastName = "Drake", Department = "HR", Email = "ipsum.sodales@partur.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "S.", Phone = "1-304-654-9033",SSN = "440921401", DriverLicense = "118259-9678" },
    new Employee { FirstName = "Amelia", LastName = "Solis", Department = "HR", Email = "risus.Morbi.metus@lorem.ca", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-843-281-5972",SSN = "800505947", DriverLicense = "414922-5080" },
    new Employee { FirstName = "Joan", LastName = "Bender", Department = "HR", Email = "ac@rhoncusDonecest.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-800-747-5568",SSN = "111103370", DriverLicense = "123791-7289" },
    new Employee { FirstName = "Kim", LastName = "Newman", Department = "Accounting", Email = "Vivamus@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-716-775-6475",SSN = "420203763", DriverLicense = "767045-2882" },
    new Employee { FirstName = "Daniel", LastName = "Blanchard", Department = "TechSupport", Email = "Proin@felis.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-627-895-5767",SSN = "161221319", DriverLicense = "107672-9308" },
    new Employee { FirstName = "Irene", LastName = "Santiago", Department = "HR", Email = "et@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "A.", Phone = "1-949-415-0524",SSN = "710807438", DriverLicense = "236602-6868" },
    new Employee { FirstName = "Abdul", LastName = "Lowe", Department = "HR", Email = "molestie.Sed.id@amet.org", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-197-283-0918",SSN = "280618275", DriverLicense = "749213-7349" },
    new Employee { FirstName = "Grant", LastName = "Byrd", Department = "TechSupport", Email = "tempor.augue@Duis.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "D.", Phone = "1-187-852-7070",SSN = "640106056", DriverLicense = "698436-7646" },
    new Employee { FirstName = "Tana", LastName = "Joyner", Department = "HR", Email = "pede.sagittis@pretium.edu", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-695-713-4689",SSN = "760313223", DriverLicense = "944158-4977" },
    new Employee { FirstName = "Jade", LastName = "Barry", Department = "HR", Email = "est@gmail.com", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-767-777-4166",SSN = "000423175", DriverLicense = "086002-4702" },
    new Employee { FirstName = "Austin", LastName = "Gentry", Department = "TechSupport", Email = "Donec@nuncsed.co.uk", PictureUrl = "http://i.pravatar.cc/", MiddleName = "B.", Phone = "1-498-880-2240",SSN = "100524534", DriverLicense = "742631-9138" },
    new Employee { FirstName = "Olga", LastName = "Owens", Department = "Accounting", Email = "amet@cursus.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "R.", Phone = "1-608-240-3818",SSN = "980218750", DriverLicense = "145169-8904" },
    new Employee { FirstName = "Cathleen", LastName = "Dalton", Department = "HR", Email = "convallis@commodo.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-697-303-8618",SSN = "841225368", DriverLicense = "023434-9405" },
    new Employee { FirstName = "Jerome", LastName = "Frost", Department = "Accounting", Email = "rhoncus@piscing.net", PictureUrl = "http://i.pravatar.cc/", MiddleName = "E.", Phone = "1-250-143-2694",SSN = "740213715", DriverLicense = "245834-5515" }
                    };

                    context.Employees.AddRange(Emps);
                    context.SaveChanges();
                    }
                }

            }
        }

    }
