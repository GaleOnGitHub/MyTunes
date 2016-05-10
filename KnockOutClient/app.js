'use strict;'
var ViewModel = function () {
    var self = this;
    self.pageNumber = 0;
    self.artists = ko.observableArray();
    self.error = ko.observable();
    self.artistDetails = ko.observable();
    self.albumDetails = ko.observable();
    self.trackDetails = ko.observable();
    
    var apiUri = 'http://localhost:50420/api/';
    var artistsUri = apiUri+'/ArtistsAPI/';
    var albumsUri = apiUri+'/AlbumsAPI/';
    var tracksUri = apiUri+'/TracksAPI/';
    
    // Fetch the initial data.
    getAllArtists();
    
    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllArtists() {
        ajaxHelper(artistsUri+"?page="+self.pageNumber, 'GET').done(function (data) {
            self.artists(data);
        });
    }
    self.pageNextArtists = function(){
        ++self.pageNumber;
        getAllArtists();
    }
    self.pagePreviousArtists = function(){
        if(self.pageNumber > 0){
            --self.pageNumber;
        }
        getAllArtists()
    }
    
    self.getArtistDetails = function (item) {
        self.albumDetails(null);
        self.trackDetails(null);
        ajaxHelper(artistsUri + item.Id, 'GET').done(function (data) {
            self.artistDetails(data);
        });
    }
    
    self.getAlbumDetails = function (item) {
        self.trackDetails(null);
        ajaxHelper(albumsUri + item.Id, 'GET').done(function (data) {
            self.albumDetails(data);
        });
    }
    
    self.getTrackDetails = function (item) {
        ajaxHelper(tracksUri + item.Id, 'GET').done(function (data) {
            self.trackDetails(data);
        });
    }
};

ko.applyBindings(new ViewModel());