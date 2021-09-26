# ProjetC
![GitHub](https://img.shields.io/github/license/Dalto1/ProjetC)
![GitHub last commit](https://img.shields.io/github/last-commit/Dalto1/ProjetC)
![Lines of code](https://img.shields.io/tokei/lines/github/Dalto1/ProjetC)

Projet personnel simulant des comptes et des opérations diverses.

## RESTful API
|                           	| POST                       	| GET                                                 	| PUT                           	| DELETE                                      	|
|---------------------------	|----------------------------	|-----------------------------------------------------	|-------------------------------	|---------------------------------------------	|
| api/comptes               	| Création compte            	| Informations sur tous les comptes                   	| Erreur                        	| Effacer tous les comptes                    	|
| api/comptes/1             	| Erreur                     	| Information sur un compte                           	| Mise à jour du compte         	| Effacer le compte                           	|
| api/comptes/1/transactions 	| Erreur                     	| Information sur toutes les transactions d'un compte 	| Erreur                        	| Effacer toutes les transactions d'un compte 	|
| api/transactions           	| Création d'une transaction 	| Informations sur toutes les transactions            	| Erreur                        	| Effacer toutes les transactions             	|
| api/transactions/1         	| Erreur                     	| Information sur une transaction                     	| Mise à jour d'une transaction 	| Effacer une transaction                     	|

## GRPC API
### Accounts
|                             	| Input                 	| Output                	|
|-----------------------------	|-----------------------	|-----------------------	|
| AccountCreate               	| ProtoAccountModel     	| ProtoAccountModel     	|
| AccountList                 	| google.protobuf.Empty 	| ProtoAccountModel     	|
| AccountDeleteAll            	| google.protobuf.Empty 	| google.protobuf.Empty 	|
| AccountFind                 	| ProtoAccountNumber    	| ProtoAccountModel     	|
| AccountUpdate               	| ProtoAccountModel     	| ProtoAccountModel     	|
| AccountDelete               	| ProtoAccountNumber    	| google.protobuf.Empty 	|
| GetTransactionsByAccount    	| ProtoAccountNumber    	| ProtoAccountModel     	|
| DeleteTransactionsByAccount 	| ProtoAccountNumber    	| google.protobuf.Empty 	|

### Transactions
|                      	| Input                  	| Output                	|
|----------------------	|------------------------	|-----------------------	|
| TransactionCreate    	| ProtoTransactionModel  	| ProtoTransactionModel 	|
| TransactionList      	| google.protobuf.Empty  	| ProtoTransactionModel 	|
| TransactionDeleteAll 	| google.protobuf.Empty  	| google.protobuf.Empty 	|
| TransactionFind      	| ProtoTransactionNumber 	| ProtoTransactionModel 	|
| TransactionUpdate    	| ProtoTransactionModel  	| ProtoTransactionModel 	|
| TransactionDelete    	| ProtoTransactionNumber 	| google.protobuf.Empty 	|

## Fonctionalités partielles et futures
* Tests unitaires
* Tests Podman
* GRPC
* Isolation dans un domaine

## License

* [GNU GPL v3](http://www.gnu.org/licenses/gpl.html)
* Copyright 2021
