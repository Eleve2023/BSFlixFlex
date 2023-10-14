# Documentation de l'API

## Routes

### GET /api/movie/top_rated
Renvoie les films les mieux not�s. 
- Param�tres : `page` (facultatif, par d�faut � 1)
- Exemple : `/api/movie/top_rated?page=2`

### GET /api/tvshow/top_rated
Renvoie les �missions de t�l�vision les mieux not�es.
- Param�tres : `page` (facultatif, par d�faut � 1)
- Exemple : `/api/tvshow/top_rated?page=2`

### GET /api/movie
Renvoie une liste de films.
- Param�tres : `page` (facultatif, par d�faut � 1)
- Exemple : `/api/movie?page=2`

### GET /api/tvshow
Renvoie une liste d'�missions de t�l�vision.
- Param�tres : `page` (facultatif, par d�faut � 1)
- Exemple : `/api/tvshow?page=2`

### GET /api/search/movie
Recherche des films.
- Param�tres : `search` (requis), `page` (facultatif, par d�faut � 1)
- Exemple : `/api/search/movie?search=inception&page=2`

### GET /api/search/tvshow
Recherche des �missions de t�l�vision.
- Param�tres : `search` (requis), `page` (facultatif, par d�faut � 1)
- Exemple : `/api/search/tvshow?search=friends&page=2`

### GET /api/movie/{id}
Renvoie les d�tails d'un film sp�cifique.
- Param�tres : `id` (requis)
- Exemple : `/api/movie/550`

### GET /api/tvshow/{id}
Renvoie les d�tails d'une �mission de t�l�vision sp�cifique.
- Param�tres : `id` (requis)
- Exemple : `/api/tvshow/1399`

### GET /api/myfavorites
Renvoie une liste de favoris pour l'utilisateur actuellement connect�.
- Exemple : `/api/myfavorites`

### GET /api/myfavorites/{id}
Renvoie une valeur sp�cifique.
- Param�tres : `id` (requis)
- Exemple : `/api/myfavorites/1`

### POST /api/myfavorites
Ajoute un favori pour l'utilisateur actuellement connect�.
- Param�tres : `id` (requis), `cinematography` (requis)
- Exemple : `/api/myfavorites?id=1&cinematography=movie`

### PUT /api/myfavorites/{id}
Mise � jour d'une valeur sp�cifique. Cette route ne semble pas �tre utilis�e actuellement.
- Param�tres : `id` (requis), `value` (requis dans le corps de la requ�te)
- Exemple : `/api/myfavorites/1`

### DELETE /api/myfavorites/{id}
Supprime un favori pour l'utilisateur actuellement connect�.
- Param�tres : `id` (requis), `cinematography` (requis)
- Exemple : `/api/myfavorites?id=1&cinematography=movie`

### POST /login
Connecte un utilisateur existant.
- Param�tres : `Email`, `Password`
- Exemple : `/login`


### POST /register
Cr�e un nouvel utilisateur.
- Param�tres : `Email`, `Password`, `UserName`
- Exemple : `/register`