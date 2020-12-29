--Part 1

	Column		Data type
	======		=========
	Id			int
	Name		longtext
	EmployerId	int

--Part 2

	SELECT Name FROM employers
	WHERE Location = "St. Louis City";

--Part 3

	SELECT name, description
	FROM skills
	INNER JOIN jobskills ON jobskills.SkillId = skills.Id
	ORDER BY name ASC
