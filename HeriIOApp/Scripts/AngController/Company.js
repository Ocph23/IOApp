/// <reference path="../angular.min.js" />
/// Home

var app = angular.module('Company', ['ngRoute', 'ui.tinymce'])

    .directive('dir', function ($compile, $parse) {
        return {
            restrict: 'E',
            link: function (scope, element, attr) {
                scope.$watch(attr.content, function () {
                    element.html($parse(attr.content)(scope));
                    $compile(element.contents())(scope);
                }, true);
            }
        }
    })

.factory("CompanyService", function ($rootScope,$http) {
    var service = {};
    var url = "/api/Inbox/GetInboxCount";
    $http({
        method: 'GET',
        url: url,
    }).success(
        function (data, status, header, cfg) {
            $rootScope.InboxCount = data;
        }
    ).error(function (err, status) {
        $rootScope.InboxCount = 0;
    });


    service.ReadMessage = function (message) {
        var url = "/api/Inbox/ReadMessage?id="+message.Id;
        $http({
            method: 'Get',
            url: url,
        }).success(
            function (data, status, header, cfg) {
                message.Terbaca = true;
            }
        ).error(function (err, status) {
            $rootScope.InboxCount = 0;
        });

    }

    service.tinymceOptions = {
        selector: "textarea",
        plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime media table contextmenu paste ',
                'textcolor'
        ],
        toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image responsivefilemanager | code | fontselect | forecolor backcolor',
        font_formats: 'Arial=arial,helvetica,sans-serif;Courier New=courier new,courier,monospace;AkrutiKndPadmini=Akpdmi-n' + "Andale Mono=andale mono,times;" +
               "Arial=arial,helvetica,sans-serif;" +
               "Arial Black=arial black,avant garde;" +
               "Book Antiqua=book antiqua,palatino;" +
               "Comic Sans MS=comic sans ms,sans-serif;" +
               "Courier New=courier new,courier;" +
               "Georgia=georgia,palatino;" +
               "Helvetica=helvetica;" +
               "Impact=impact,chicago;" +
               "Symbol=symbol;" +
               "Tahoma=tahoma,arial,helvetica,sans-serif;" +
               "Terminal=terminal,monaco;" +
               "Times New Roman=times new roman,times;" +
               "Trebuchet MS=trebuchet ms,geneva;" +
               "Verdana=verdana,geneva;" +
               "Webdings=webdings;" +
               "Wingdings=wingdings,zapf dingbats",
        image_advtab: true,

    };

    return service;

})


.factory("ProfileService", function ($rootScope, $http,$q) {
    var service = {};

    service.Profile = null;
    service.promise = asyncInit();
   function asyncInit() {
        var deferred = $q.defer();

        setTimeout(function () {
            var url = "/api/Profile/Get";
            $http({
                method: 'GET',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    service.Profile = data;
                    deferred.resolve(data);
                }
            ).error(function (err, status) {
                service.Profile = null;
                deferred.reject(null);
            });
        }, 1000);

        return deferred.promise;
    }

    service.GetProfile=function()
    {
        return service.Profile;
    }
    service.ShowImage = function (source, destination) {
        var file = document.getElementById(source);
        var ofReader = new FileReader();
        ofReader.readAsDataURL(document.getElementById(source).files[0]);
        ofReader.onload = function (oFREvent) {
            document.getElementById(destination).src = oFREvent.target.result;
        };
    }



    return service;

})
    
.config(function ($routeProvider) {
    $routeProvider.
        when('/Index', {
            templateUrl: 'MenuView.htm',
            controller: 'MenuController'
        }).
           when('/', {
               templateUrl: 'MenuView.htm',
               controller: 'MenuController'
           }).
         when('/Layanan', {
             templateUrl: 'LayananView.htm',
             controller: 'LayananController'
         }).

          when('/Pesanan', {
              templateUrl: 'PesananView.htm',
              controller: 'PesananController'
          }).

         when('/Penawaran', {
              templateUrl: 'PenawaranView.htm',
              controller: 'PenawaranController'
         }).
         when('/Penawaran/:Id', {
             templateUrl: 'PenawaranBidView.htm',
             controller: 'PenawaranController'
         }).




      when('/Inbox', {
          templateUrl: 'InboxView.htm',
          controller: 'InboxController'
      });

})

 .controller('MenuController', function ($scope, $http, CompanyService, ProfileService,$sce,$rootScope) {
     $scope.tinymceOptions = CompanyService.tinymceOptions;
     $scope.Judul = "Menu Utama";
     $scope.serviceProfile = ProfileService;
     $scope.UserProfile = {};
     ProfileService.promise.then(function (profile) {
         $scope.UserProfile = profile;
         $rootScope.UserPhoto = profile.UserPhoto;
         $scope.ShowSloganModel = $sce.trustAsHtml($scope.UserProfile.Selogan);
         $scope.ShowDescriptionModel = $sce.trustAsHtml($scope.UserProfile.Description);
     }, function (reason) {
         alert('Failed: ' + reason);
     }, function (update) {
         alert('Got notification: ' + update);
     });



     $scope.UpdateUserPhoto=function(source,destination)
     {
         ProfileService.ShowImage(source, destination);
         var f = document.getElementById(source);

         var res = f.files[0];
         var form = new FormData();
         form.append("file", res);
         form.append("Id", $scope.UserProfile.Id);
         form.append("UserId", $scope.UserProfile.UserId);
         form.append("UserType", $scope.UserProfile.UserType);
         

         var settings = {
             "async": true,
             "crossDomain": true,
             "url": "/api/Profile/UpdateUserPhoto",
             "method": "POST",
             "headers": {
                 "cache-control": "no-cache",
             },
             "processData": false,
             "contentType": false,
             "mimeType": "multipart/form-data",
             "data": form
         }

         $.ajax(settings).done(function (response, data) {
             alert("Berhasil Diubah");
         }).error(function (response, data) {
             alert(response.responseText);
         });
     }


     $scope.UpdatePageImage = function (source, destination) {
         ProfileService.ShowImage(source, destination);
         var f = document.getElementById(source);

         var res = f.files[0];
         var form = new FormData();
         form.append("file", res);
         form.append("Id", $scope.UserProfile.Id);
         form.append("UserId", $scope.UserProfile.UserId);
         form.append("UserType", $scope.UserProfile.UserType);


         var settings = {
             "async": true,
             "crossDomain": true,
             "url": "/api/Profile/UpdatePageImage",
             "method": "POST",
             "headers": {
                 "cache-control": "no-cache",
             },
             "processData": false,
             "contentType": false,
             "mimeType": "multipart/form-data",
             "data": form
         }

         $.ajax(settings).done(function (response, data) {
             alert("Berhasil Diubah");
         }).error(function (response, data) {
             alert(response.responseText);
         });


     }


     $scope.EditPage=function(model)
     {
         // var url = "/api/Company/RemoveLayanan?Id=" + item.Id + "&actived=" + item.Aktif;
         $scope.SeloganIsEdit =!$scope.SeloganIsEdit;
        
     }

     $scope.ShowSloganModel;
     $scope.ShowDescriptionModel;

     $scope.OnChangeSlogan=function()
     {
         $scope.ShowSloganModel=$sce.trustAsHtml($scope.UserProfile.Selogan);
        
         var url = "/api/Profile/UpdateProfileText";
         $http({
             method: 'Post',
             url: url,
             data: $scope.UserProfile
         }).success(
             function (data, status, header, cfg) {
                 alert("Data Telah Diubah");
             }
         ).error(function (err, status) {
             alert(err.Message + ", " + status);
             item.Aktif = !item.Aktif;
         });
     }

     $scope.OnChangeDescription= function () {
         $scope.ShowDescriptionModel = $sce.trustAsHtml($scope.UserProfile.Description);
         var url = "/api/Profile/UpdateProfileText";
         $http({
             method: 'Post',
             url: url,
             data: $scope.UserProfile
         }).success(
             function (data, status, header, cfg) {
                 alert("Data Telah Diubah");
             }
         ).error(function (err, status) {
             alert(err.Message + ", " + status);
             item.Aktif = !item.Aktif;
         });
     }
     
 })

    .controller('LayananController', function ($scope, $http, CompanyService, $sce) {
        $scope.tinymceOptions = CompanyService.tinymceOptions;
        $scope.Judul = "Daftar Layanan";
        $scope.ListLayanan = [];
        $scope.Init = function () {
            var url = "/api/Company/GetLayanan";
            $http({
                method: 'GET',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    $scope.ListLayanan = data;
                }
            ).error(function (err, status) {
                alert(err.Message + ", " + status);
            });

            $scope.Categories = [];
            var url = "/api/Categories/Get";
            $http({
                method: 'GET',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    $scope.Categories = data;
                }
            ).error(function (err, status) {
                alert(err.Message + ", " + status);
            });
        }
        $scope.IsNew = true;
        $scope.SeloganIsEdit = false;
        $scope.IsSpinerShow = false;
        $scope.ChangeSelectedItem = function (item) {
            $scope.IsNew = false;
            $scope.Selecteditem = item;
            $scope.layanan = angular.copy( item);
        };
       $scope. ChangeSelectedItemDetail=function(item)
       {
           $scope.layanan = angular.copy(item);
           $scope.layanan.DescriptionHtml = $sce.trustAsHtml(item.Keterangan);
       }

        $scope.SaveLayanan=function(value)
        {
            $scope.IsSpinerShow = true;
            var form = new FormData();
            form.append("Id",value.Id)
            form.append("Nama", value.Nama);
            form.append("Stok", value.Stok);
            form.append("Harga", value.Harga);
            form.append("Keterangan",value.Keterangan);

            if($scope.IsNew)
            {
                var f = document.getElementById("file");
                var res = f.files[0];
                form.append("file", res);
                form.append("IdKategori", value.IdKategori);
                var settings = {
                    "async": true,
                    "crossDomain": true,
                    "url": "/api/Company/PostLayanan",
                    "method": "POST",
                    "headers": {
                        "cache-control": "no-cache",
                    },
                    "processData": false,
                    "contentType": false,
                    "mimeType": "multipart/form-data",
                    "data": form
                }

                $.ajax(settings).done(function (response, data) {
                    value.Id = response;
                    $scope.ListLayanan.push(value)
                    alert("Berhasil menambah data");
                    $scope.IsSpinerShow = false;
                }).error(function (response, data) {
                    $scope.IsSpinerShow = false;
                    alert(response);


                });
            } else
            {
                var settings = {
                    "async": true,
                    "crossDomain": true,
                    "url": "/api/Company/UpdateLayanan",
                    "method": "POST",
                    "headers": {
                        "cache-control": "no-cache",
                    },
                    "processData": false,
                    "contentType": false,
                    "mimeType": "multipart/form-data",
                    "data": form
                }

                $.ajax(settings).done(function (response, data) {
                    $scope.Selecteditem.Nama = value.Nama;
                    $scope.Selecteditem.Stok = value.Stok;
                    $scope.Selecteditem.Harga = value.Harga;
                    $scope.Selecteditem.Keterangan = value.Keterangan;
                    $scope.Selecteditem.DescriptionHtml = $sce.trustAsHtml(value.Keterangan);
                    $scope.IsSpinerShow = false;
                    alert("Berhasil menambah data");
                }).error(function (response, data) {
                    $scope.IsSpinerShow = false;
                    alert(response);
                });
            }

          
        }


        $scope.RemoveLayanan = function (item) {
            var url = "/api/Company/RemoveLayanan?Id="+item.Id+"&actived="+item.Aktif;
            $http({
                method: 'Post',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    $scope.Categories = data;
                }
            ).error(function (err, status) {
                alert(err.Message + ", " + status);
            });

        }


        $scope.ChangeLayananActived = function (item) {
            var url = "/api/Company/ChangeLayananActived";
            $http({
                method: 'Post',
                url: url,
                data:item
            }).success(
                function (data, status, header, cfg) {
                    alert("Data Telah Diubah");
                }
            ).error(function (err, status) {
                alert(err.Message + ", " + status);
                item.Aktif = !item.Aktif;
            });
        }

    })


 .controller('PenawaranController', function ($scope, $http, CompanyService, $route, $routeParams,$location,$sce) {
     $scope.Init = function () {
         var url = "";
         if($routeParams.Id !== undefined)
         {
             $scope.Judul = "Pengajuan Penawaran";
             var id = $routeParams.Id;
             url = "/api/Company/GetPesananById?Id="+id;
             $http({
                 method: 'GET',
                 url: url,
             }).success(
                 function (data, status, header, cfg) {
                     $scope.Pesanan = angular.fromJson(data);

                     if($scope.Pesanan.Bid !== null)
                     {
                         $scope.Message = "Anda Telah Mengajukan Penawaran Untuk Event Ini";
                         $scope.IsMessageShow = true;
                     }else if ($scope.Pesanan.Pemesanan.StatusPesanan === "Batal")
                     {
                         $scope.Message = "Pesanan Telah Dibatalkan";
                         $scope.IsMessageShow = true;
                     } else if ($scope.Pesanan.Pemesanan.VerifikasiPembayaran !== "Batal" && $scope.Pesanan.Pemesanan.StatusPesanan !== "Baru") {
                         $scope.Message = "Masa Pengajuan Penawaran Telah Berakhir";
                         $scope.IsMessageShow = true;
                     }else
                     {
                         $scope.IsShowBid = true;
                     }
                 }
             ).error(function (err, status) {
                 alert(err.Message + ", " + status);
                 $location.path("Penawaran");
             });



         } else
         {
             $scope.Judul = "Daftar Penawaran";
             url = "/api/Company/GetMyPenawaran";
             $http({
                 method: 'GET',
                 url: url,
             }).success(
                 function (data, status, header, cfg) {
                     $scope.Penawarans = data;
                 }


             ).error(function (err, status) {
                 alert(err.Message + ", " + status);
             });
         }



         
     }

     $scope.SelectedItemAction=function(item)
     {
         $scope.Selecteditem = {};
         $scope.Selecteditem = item;
         $scope.Selecteditem.DetailPenawaranHtml = $sce.trustAsHtml(item.DetailPenawaran);
     }  

     $scope.Bid=function(model)
     {
         model.IdPemesanan = $scope.Pesanan.Pemesanan.Id;
         var url = "/api/Company/PostPenawaran";
         $http({
             method: 'post',
             url: url,
             data: model
         })
             .success(function (data, Status) {
                 $location.path("Penawaran");
             })

             .error(function (err, status) {
                 alert(err);
         });
     }

     $scope.ChangeProgress = function (item) {
         var url = "/api/Company/ChangeProgressPenawaran";
         $http({
             method: 'post',
             url: url,
             data: item
         }).error(function (err, status) {
         });
     }


 })

 .controller('PesananController', function ($scope, $http, CompanyService) {
     $scope.Judul = "Daftar Pesanan";
     $scope.Init = function () {
         var url = "/api/Company/GetPesanan";
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.Pesanan = data;
                 angular.forEach($scope.Pesanan, function (value, key) {
                     value.StatusPesananText = CompanyService.ConvertVerifikasiPembayaran(value.VerifikasiPembayaran);
                 })
             }


         ).error(function (err, status) {
             alert(err.Message + ", " + status);
         });
     }


     $scope.ChangeProgress=function(item)
     {
         var url = "/api/Company/ChangeProgress";
         $http({
             method: 'post',
             url: url,
             data:item
         }).error(function (err, status) {
         });
     }
    


 })

 .controller('InboxController', function ($scope, $http, CompanyService, $sce, $location) {
     $scope.Judul = "Inbox";
     $scope.MessageISShow = false;
     $scope.Init = function () {
         var url = "/api/Inbox/GetInbox";
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.Inbox = data

             });
     };

     $scope.Bid = function (Id) {
         var a = Id;
         $('#modal-container-488721').modal('hide');
         $location.path("Penawaran/"+Id);
     };

     $scope.ShowMessage = function (item) {
         $scope.SelectedMessage= {};
         $scope.SelectedMessage = item;
         $scope.SelectedMessage.HtmlPesan = $sce.trustAsHtml(item.Pesan);
         CompanyService.ReadMessage(item);
     }
 });

