count=20
for i in $(seq $count); do
	curl --request "DELETE" http://localhost:3001/admin/delete/logs 
	sleep 5m
done
