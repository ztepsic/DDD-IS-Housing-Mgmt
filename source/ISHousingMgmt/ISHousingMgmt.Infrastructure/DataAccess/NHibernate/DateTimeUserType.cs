using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace ISHousingMgmt.Infrastructure.DataAccess.NHibernate {
	[Serializable]
	class DateTimeUserType : IUserType {

		#region Members

		private static readonly SqlType[] sqlTypes = {NHibernateUtil.DateTime.SqlType};

		/// <summary>
		/// The SQL types for the columns mapped by this type. 
		/// </summary>
		public SqlType[] SqlTypes {
			get { return sqlTypes; }
		}

		/// <summary>
		/// The type returned by <c>NullSafeGet()</c>
		/// </summary>
		public Type ReturnedType {
			get { return typeof (DateTimeUserType); }
		}

		/// <summary>
		/// Are objects of this type mutable?
		/// </summary>
		public bool IsMutable {
			get { return false; }
		}

		#endregion

		#region Constructors and Init
		#endregion

		#region Methods

		/// <summary>
		/// Retrieve an instance of the mapped class from a JDBC resultset.
		///             Implementors should handle possibility of null values.
		/// </summary>
		/// <param name="rs">a IDataReader</param><param name="names">column names</param><param name="owner">the containing entity</param>
		/// <returns/>
		/// <exception cref="T:NHibernate.HibernateException">HibernateException</exception>
		public object NullSafeGet(IDataReader rs, string[] names, object owner) {
			object obj = NHibernateUtil.DateTime.NullSafeGet(rs, names[0]);
			if (obj == null) {
				return null;
			}

			DateTime dateTime = (DateTime) obj;
			DateTime dateTime2 = dateTime;
			if(dateTime.Year == 1753 && dateTime.Month == 1 && dateTime.Day == 1 
				&& dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0) {
				dateTime2 = dateTime.AddYears(-1752); // 1753 - 1752 = 0001;	
			}

			return dateTime2;
		}

		/// <summary>
		/// Write an instance of the mapped class to a prepared statement.
		///             Implementors should handle possibility of null values.
		///             A multi-column type should be written to parameters starting from index.
		/// </summary>
		/// <param name="cmd">a IDbCommand</param><param name="value">the object to write</param><param name="index">command parameter index</param><exception cref="T:NHibernate.HibernateException">HibernateException</exception>
		public void NullSafeSet(IDbCommand cmd, object value, int index) {
			if(value == null) {
				((IDataParameter) cmd.Parameters[index]).Value = DBNull.Value;
			} else {
				DateTime dateTime2 = (DateTime) value;
				DateTime dateTime = dateTime2;
				if(dateTime2.Year == 1 && dateTime2.Month == 1 && dateTime2.Day == 1 &&
					dateTime2.Hour == 0 && dateTime2.Minute == 0 && dateTime2.Second == 0 && dateTime2.Millisecond == 0) {
					dateTime = dateTime2.AddYears(1752); // 0001 + 1752 = 1753	
				}

				((IDataParameter) cmd.Parameters[index]).Value = dateTime;
			}
		}

		/// <summary>
		/// During merge, replace the existing (<paramref name="target"/>) value in the entity
		///             we are merging to with a new (<paramref name="original"/>) value from the detached
		///             entity we are merging. For immutable objects, or null values, it is safe to simply
		///             return the first parameter. For mutable objects, it is safe to return a copy of the
		///             first parameter. For objects with component values, it might make sense to
		///             recursively replace component values.
		/// </summary>
		/// <param name="original">the value from the detached entity being merged</param><param name="target">the value in the managed entity</param><param name="owner">the managed entity</param>
		/// <returns>
		/// the value to be merged
		/// </returns>
		public object Replace(object original, object target, object owner) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reconstruct an object from the cacheable representation. At the very least this
		///             method should perform a deep copy if the type is mutable. (optional operation)
		/// </summary>
		/// <param name="cached">the object to be cached</param><param name="owner">the owner of the cached object</param>
		/// <returns>
		/// a reconstructed object from the cachable representation
		/// </returns>
		public object Assemble(object cached, object owner) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Transform the object into its cacheable representation. At the very least this
		///             method should perform a deep copy if the type is mutable. That may not be enough
		///             for some implementations, however; for example, associations must be cached as
		///             identifier values. (optional operation)
		/// </summary>
		/// <param name="value">the object to be cached</param>
		/// <returns>
		/// a cacheable representation of the object
		/// </returns>
		public object Disassemble(object value) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Return a deep copy of the persistent state, stopping at entities and at collections.
		/// </summary>
		/// <param name="value">generally a collection element or entity field</param>
		/// <returns>
		/// a copy
		/// </returns>
		public object DeepCopy(object value) {
			return value;
		}

		/// <summary>
		/// Compare two instances of the class mapped by this type for persistent "equality"
		///             ie. equality of persistent state
		/// </summary>
		/// <param name="x"/><param name="y"/>
		/// <returns/>
		public new bool Equals(object x, object y) {
			if (x == y) {
				return true;
			}

			if (x == null || y == null) {
				return false;
			}

			return x.Equals(y);
		}

		/// <summary>
		/// Get a hashcode for the instance, consistent with persistence "equality"
		/// </summary>
		public int GetHashCode(object x) {
			return x.GetHashCode();
		}

		#endregion		
	}
}
