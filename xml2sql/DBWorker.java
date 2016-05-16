package com.patch.database;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

/**
 * Created by maks on 5/9/2016.
 */
public class DBWorker {

    private static final String URL = "jdbc:mysql://localhost:3306/patchdb";
    private static final String USERNAME = "root";
    private static final String PASSWORD = "password";

    private Connection connection;

    public DBWorker(){
        try {
            connection = DriverManager.getConnection(URL,USERNAME,PASSWORD);
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public Connection getConnection() {
        return connection;
    }
}
