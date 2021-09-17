# ProjetC
![GitHub](https://img.shields.io/github/license/Dalto1/ProjetC)
![GitHub last commit](https://img.shields.io/github/last-commit/Dalto1/ProjetC)
![Lines of code](https://img.shields.io/tokei/lines/github/Dalto1/ProjetC)

Projet personnel simulant des comptes et des opérations diverses.

## RESTful API
|                 	| POST                    	| GET                                  	| PUT                        	| DELETE                      	|
|-----------------	|-------------------------	|--------------------------------------	|----------------------------	|-----------------------------	|
| api/comptes     	| Création compte         	| Informations sur tous les comptes    	|                            	| Effacer tous les comptes    	|
| api/comptes/1   	| Erreur                  	| Information sur un compte            	| Mise à jour du compte      	| Effacer le compte           	|
| api/transfert   	| Création d'un transfert 	| Informations sur tous les transferts 	|                            	| Effacer tous les transferts 	|
| api/transfert/1 	| Erreur                  	| Information sur un transfert         	| Mise à jour d'un transfert 	| Effacer un transfert        	|

## Fonctionalités partielles et futures
* Transfert effectif dans les comptes
* Associer les transactions dans les comptes
* Tests unitaires
* Tests Podman
* GRPC
* Isolation dans un domaine

## License

* [GNU GPL v3](http://www.gnu.org/licenses/gpl.html)
* Copyright 2021
