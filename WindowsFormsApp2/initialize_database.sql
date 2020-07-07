create table currencies(
    code varchar(4),
    title varchar(255),
    PRIMARY KEY (code)
);

create table currency_rates(
    code varchar(4), 
    bank_id varchar(255),
    purchase_rate float, 
    selling_rate float, 
    date_time datetime,
    PRIMARY KEY (code, date_time, bank_id),
    FOREIGN KEY (code) REFERENCES currencies(code),
    FOREIGN KEY (bank_id) REFERENCES banks(id)
);

create table banks(
    id varchar(255),
    name varchar(255),
    PRIMARY KEY (id)
);