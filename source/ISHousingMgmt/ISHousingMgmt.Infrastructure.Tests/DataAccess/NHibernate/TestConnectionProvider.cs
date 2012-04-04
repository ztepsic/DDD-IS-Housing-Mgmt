using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using NHibernate.Connection;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate {
	class TestConnectionProvider : ConnectionProvider {

		private static SQLiteConnection connection = null;

		/// <summary>
		/// Get an open <see cref="T:System.Data.IDbConnection"/>.
		/// </summary>
		/// <returns>
		/// An open <see cref="T:System.Data.IDbConnection"/>.
		/// </returns>
		public override IDbConnection GetConnection() {
			if(connection == null) {
				// new connection
				connection = new SQLiteConnection(ConnectionString);
			}

			if(connection.State != ConnectionState.Open) {
				connection.Open();	
			}
			
			return connection;

		}

		public override void CloseConnection(IDbConnection conn) {
			// ignore closing the connection
			// connection'll be closed by calling CloseDatabase by the and of TestFixture
		}

		public static void CloseDatabase() {
			if(connection != null) {
				connection.Close();
			}
		}
	}
}
